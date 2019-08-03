using System.IO.Ports;
using System.Threading;

namespace X10Test
{
    public class X10Utils
    {
        private string port = "COM3";
        private int baud = 115200;
        private int databits = 8;
        private Parity parity = Parity.None;
        private StopBits stopbits = StopBits.One;

        private SerialPort serialPort;

        public X10Utils()
        {
            serialPort = new SerialPort(port, baud, parity, databits, stopbits);
        }

        //xHeader = '11010101' + '10101010';
        //xFooter = '10101101';

        //xA1on   = '01100000' + '00000000';
        //xA1off  = '01100000' + '00100000';
        /*
          xA1on   = '01100000' + '00000000';
          xA1off  = '01100000' + '00100000';
          xA2on   = '01100000' + '00010000';
          xA2off  = '01100000' + '00110000';
          xA3on   = '01100000' + '00001000';
          xA3off  = '01100000' + '00101000';
          xA4on   = '01100000' + '00011000';
          xA4off  = '01100000' + '00111000';
          xA5on   = '01100000' + '01000000';
          xA5off  = '01100000' + '01100000';
          xA6on   = '01100000' + '01010000';
          xA6off  = '01100000' + '01110000';
          xA7on   = '01100000' + '01001000';
          xA7off  = '01100000' + '01101000';
          xA8on   = '01100000' + '01011000';
          xA8off  = '01100000' + '01111000';
          xA9on   = '01100100' + '00000000';
          xA9off  = '01100100' + '00100000';
          xA10on  = '01100100' + '00010000';
          xA10off = '01100100' + '00110000';
          xA11on  = '01100100' + '00001000';
          xA11off = '01100100' + '00101000';
          xA12on  = '01100100' + '00011000';
          xA12off = '01100100' + '00111000';
          xA13on  = '01100100' + '01000000';
          xA13off = '01100100' + '01100000';
          xA14on  = '01100100' + '01010000';
          xA14off = '01100100' + '01110000';
          xA15on  = '01100100' + '01001000';
          xA15off = '01100100' + '01101000';
          xA16on  = '01100100' + '01011000';
          xA16off = '01100100' + '01111000';
          xAbright= '01100000' + '10001000';
          xAdim   = '01100000' + '10011000';
         */

        public void SendTestMessage(bool on)
        {
            var onMessage = 
                "11010101" + "10101010" +
                "01100000" + "01000000" +
                "10101101";

            var offMessage =
                "11010101" + "10101010" +
                "01100000" + "01100000" +
                "10101101";

            var message = on ? onMessage : offMessage;

            serialPort.Open();

            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            Thread.Sleep(10);

            foreach (var bit in message)
            {
                switch (bit)
                {
                    case '0':
                        serialPort.RtsEnable = false;
                        Thread.Sleep(10);
                        serialPort.RtsEnable = true;
                        Thread.Sleep(10);
                        break;
                    case '1':
                        serialPort.DtrEnable = false;
                        Thread.Sleep(10);
                        serialPort.DtrEnable = true;
                        Thread.Sleep(10);
                        break;
                    default:
                        break;
                }
            }

            Thread.Sleep(50);
            serialPort.RtsEnable = false;
            serialPort.DtrEnable = false;

            serialPort.Close();
        }

    }
}
