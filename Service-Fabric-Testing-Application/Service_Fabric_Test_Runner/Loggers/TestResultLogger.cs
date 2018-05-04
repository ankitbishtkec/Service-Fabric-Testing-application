
using System.IO;
using System.Text;

namespace Service_Fabric_Test_Runner.Loggers
{
    /*TODO:
     * https://stackoverflow.com/questions/32661000/how-can-i-use-an-own-class-for-console-writeline-override 
     * https://blogs.msdn.microsoft.com/gautamg/2009/12/08/logging-a-message-in-test-result-as-part-of-an-automated-test/
     * Write a logger which overrides Console.WriteLine() such that logs written using 'Console' during test case execution
     * are added to the 'logOutput' property of 'TestResult' object of respective test case.
     * 
     * This 'TestResult' object should also be updated to Reliable Storage periodically. So, that any query for 'TestResult' 
     * always returns object with latest logs.
     */
    class TestResultLogger : TextWriter
    {
        public override void Write(char value)
        {
            //Do something, like write to a file or something
        }

        public override void Write(string value)
        {
            //Do something, like write to a file or something
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.ASCII;
            }
        }
    }
}
