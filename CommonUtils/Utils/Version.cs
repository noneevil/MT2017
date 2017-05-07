using System.Reflection;

namespace CommonUtils
{
    public abstract class Version
    {
        public static int version
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                System.Version version = assembly.GetName().Version;
                return version.Major;
            }
        }
    }

}
