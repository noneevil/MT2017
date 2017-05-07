using System;
using System.Threading;
using SpeechLib;
/*
 * 示例:
 *   SpVoiceHelper sp = new SpVoiceHelper();
 *   sp.InputText = "测试";
 *   sp.Format = SpeechAudioFormatType.SAFT8kHz8BitStereo;
 *   sp.SaveFileName = Server.MapPath("/test.wav");
 *   sp.Rate = 0;
 *   sp.Volume = 90;
 *   sp.Convert();
 */
namespace CommonUtils
{
    /// <summary>
    /// 文本转语音扩展类
    /// 引用COM组件:Microsoft Speech Object Library
    /// </summary>
    public class SpVoiceHelper
    {
        /// <summary>
        /// 保存wav文件
        /// </summary>
        public String SaveFileName { get; set; }
        /// <summary>
        /// 要转换文件
        /// </summary>
        public String InputText { get; set; }
        /// <summary>
        /// 朗读速度
        /// </summary>
        public int Rate { get; set; }
        /// <summary>
        /// 音量大小(1-100)
        /// </summary>
        public int Volume { get; set; }
        /*音质参数参照
                参数                   文件大小
        SAFT48kHz16BitStereo	        1.13M
        SAFT44kHz16BitStereo	        1.04M
        SAFT32kHz16BitStereo	        772.28K
        SAFT24kHz16BitStereo	        579.22K
        SAFT48kHz8BitStereo		        579.18K
        SAFT48kHz16BitMono		        579.18K
        SAFT22kHz16BitStereo	        532.16K
        SAFT44kHz8BitStereo		        532.12K
        SAFTCCITT_uLaw_44kHzStereo		532.12K
        SAFTCCITT_ALaw_44kHzStereo		532.12K
        SAFT44kHz16BitMono		        532.12K
        SAFT32kHz8BitStereo		        386.16K
        SAFT32kHz16BitMono		        386.16K
        SAFT16kHz16BitStereo	        386.16K
        SAFT24kHz16BitMono		        289.63K
        SAFT24kHz8BitStereo		        289.63K
        SAFT12kHz16BitStereo	        289.63K
        SAFT48kHz8BitMono		        289.61K
        SAFTADPCM_44kHzStereo	        267.69K
        SAFTCCITT_ALaw_22kHzStereo	    266.1K
        SAFTNoAssignedFormat		    266.1K
        SAFT22kHz8BitStereo		        266.1K
        SAFT22kHz16BitMono		        266.1K
        SAFT11kHz16BitStereo	        266.1K
        SAFTCCITT_uLaw_22kHzStereo		266.1K
        SAFTCCITT_uLaw_44kHzMono		266.08K
        SAFTCCITT_ALaw_44kHzMono		266.08K
        SAFT44kHz8BitMono		        266.08K
        SAFT16kHz16BitMono		        193.1K
        SAFT32kHz8BitMono		        193.1K
        SAFT8kHz16BitStereo		        193.1K
        SAFT16kHz8BitStereo		        193.1K
        SAFT24kHz8BitMono		        144.84K
        SAFT12kHz8BitStereo		        144.84K
        SAFT12kHz16BitMono		        144.84K
        SAFTADPCM_22kHzStereo	        134.69K
        SAFTADPCM_44kHzMono		        133.88K
        SAFTCCITT_uLaw_22kHzMono		133.07K
        SAFTCCITT_ALaw_22kHzMono		133.07K
        SAFTCCITT_ALaw_11kHzStereo		133.07K
        SAFTCCITT_uLaw_11kHzStereo		133.07K
        SAFT22kHz8BitMono		        133.07K
        SAFT11kHz16BitMono		        133.07K
        SAFT11kHz8BitStereo		        133.07K
        SAFT8kHz8BitStereo		        96.57K
        SAFTCCITT_ALaw_8kHzStereo       96.57K
        SAFT16kHz8BitMono		        96.57K
        SAFT8kHz16BitMono		        96.57K
        SAFTCCITT_uLaw_8kHzStereo		96.57K
        SAFT12kHz8BitMono		        72.44K
        SAFTADPCM_11kHzStereo	        68.2K
        SAFTADPCM_22kHzMono		        67.38K
        SAFT11kHz8BitMono		        66.56K
        SAFTCCITT_ALaw_11kHzMono	    66.56K
        SAFTCCITT_uLaw_11kHzMono	    66.56K
        SAFTGSM610_44kHzMono		    54.13K
        SAFTADPCM_8kHzStereo		    49.08K
        SAFTCCITT_ALaw_8kHzMono		    48.31K
        SAFTCCITT_uLaw_8kHzMono		    48.31K
        SAFT8kHz8BitMono		        48.31K
        SAFTADPCM_11kHzMono		        34.14K
        SAFTGSM610_22kHzMono	        27.09K
        SAFTADPCM_8kHzMono		        24.58K
        SAFTGSM610_11kHzMono	        13.5K
        SAFTGSM610_8kHzMono		        9.82K
        */
        /// <summary>
        /// 语音质量
        /// </summary>
        public SpeechAudioFormatType Format { get; set; }
        /// <summary>
        /// 开始转换
        /// </summary>
        public void Convert()
        {
            SpFileStream sr = new SpFileStream();
            SpAudioFormat audio = new SpAudioFormat();
            audio.Type = Format;
            sr.Format = audio;
            sr.Open(SaveFileName, SpeechStreamFileMode.SSFMCreateForWrite, false);

            SpVoice voice = new SpVoice();
            voice.Voice = voice.GetVoices(null, null).Item(0);//语言设置
            voice.Rate = Rate;
            voice.AudioOutputStream = sr;
            voice.Volume = Volume == 0 ? 100 : Volume;
            voice.Speak(InputText, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            voice.WaitUntilDone(Timeout.Infinite);
            sr.Close();
        }
    }
}
