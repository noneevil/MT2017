/*
 * CKFinder
 * ========
 * http://cksource.com/ckfinder
 * Copyright (C) 2007-2013, CKSource - Frederico Knabben. All rights reserved.
 *
 * The software, this file and its contents are subject to the CKFinder
 * License. Please read the license.txt file before using, installing, copying,
 * modifying or distribute this file or part of its contents. The contents of
 * this file is part of the Source Code of CKFinder.
 */

using System;
using System.Web;
using WebSite.Controls.Connector;

namespace WebSite.Controls.Connector.CommandHandlers
{
    public class SaveFileCommandHandler : XmlCommandHandlerBase
    {
        public SaveFileCommandHandler()
            : base()
        {
        }

        protected override void BuildXml()
        {
            if (!this.CurrentFolder.CheckAcl(AccessControlRules.FileDelete))
            {
                ConnectorException.Throw(Errors.Unauthorized);
            }

            string fileName = Request["FileName"];
            string content = Request.Form["content"];

            if (!Connector.CheckFileName(fileName) || Config.Current.CheckIsHiddenFile(fileName))
            {
                ConnectorException.Throw(Errors.InvalidRequest);
                return;
            }

            if (!this.CurrentFolder.ResourceTypeInfo.CheckExtension(System.IO.Path.GetExtension(fileName)))
            {
                ConnectorException.Throw(Errors.InvalidRequest);
                return;
            }

            string filePath = System.IO.Path.Combine(this.CurrentFolder.ServerPath, fileName);

            if (!System.IO.File.Exists(filePath))
                ConnectorException.Throw(Errors.FileNotFound);

            try
            {
                System.IO.File.WriteAllText(filePath, content);
            }
            catch (System.UnauthorizedAccessException)
            {
                ConnectorException.Throw(Errors.AccessDenied);
            }
            catch (System.Security.SecurityException)
            {
                ConnectorException.Throw(Errors.AccessDenied);
            }
            catch (System.ArgumentException)
            {
                ConnectorException.Throw(Errors.FileNotFound);
            }
            catch (System.IO.PathTooLongException)
            {
                ConnectorException.Throw(Errors.FileNotFound);
            }
            catch
            {
#if DEBUG
                throw;
#else
				ConnectorException.Throw( Errors.Unknown );
#endif
            }
        }
    }
}

namespace WebSite.Controls.Plugins
{
    public class FileEditor : CKFinderPlugin
    {
        public string JavascriptPlugins
        {
            get { return "fileeditor"; }
        }

        public void Init(CKFinderEvent CKFinderEvent)
        {
            CKFinderEvent.BeforeExecuteCommand += new CKFinderEvent.Hook(this.BeforeExecuteCommand);
        }

        protected void BeforeExecuteCommand(object sender, CKFinderEventArgs args)
        {
            String command = (String)args.data[0];

            if (command == "SaveFile")
            {
                HttpResponse Response = (HttpResponse)args.data[1];

                WebSite.Controls.Connector.CommandHandlers.CommandHandlerBase commandHandler =
                    new WebSite.Controls.Connector.CommandHandlers.SaveFileCommandHandler();
                commandHandler.SendResponse(Response);
            }
        }
    }
}
