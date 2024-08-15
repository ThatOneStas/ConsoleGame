using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSexam.MsgHandlers
{
    // Цей клас існує лише як вмістилище статичних хендлерів..
    // .. за дпомогою яких, проєкт може не обмежуватися однією консолькою.
    public class Handler
    {
        // делегати
        public delegate void MessageHandler(string msg);
        public delegate void Specified1_MessageHandler(string msg);
        public delegate void Specified2_MessageHandler(string msg);
        public delegate void Specified3_MessageHandler(string msg);
        // хендлери
        public static MessageHandler? msgHandler { get; set; }
        public static Specified1_MessageHandler? specified1_msgHandler { get; set; }
        public static Specified2_MessageHandler? specified2_msgHandler { get; set; }
        public static Specified3_MessageHandler? specified3_msgHandler { get; set; }
        // методи для викликів хендлерів
        public static void Default_Print(string msg)
        {
            msgHandler?.Invoke(msg);
        }
        public static void Special1_Print(string msg)
        {
            specified1_msgHandler?.Invoke(msg);
        }
        public static void Special2_Print(string msg)
        {
            specified2_msgHandler?.Invoke(msg);
        }
        public static void Special3_Print(string msg)
        {
            specified3_msgHandler?.Invoke(msg);
        }
    }
}
