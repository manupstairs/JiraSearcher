using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestCase
{
    interface IShowWindow
    {
        event EventHandler<TestCase> ShowWindowEvent;
    }

    interface IShowMessage
    {
        event EventHandler<string> ShowMessageEvent;
    }
}
