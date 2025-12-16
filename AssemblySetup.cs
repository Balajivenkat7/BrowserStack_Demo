//Created By: Balaji  Venkatesan
//Created Date: 26 Sep 2025
using NUnit.Framework;
using UX_Automation.Core;

[SetUpFixture]
public class AssemblySetup
{
    private static WebDriverSetup _webDriverSetup;

    [OneTimeSetUp]
    public void GlobalSetUp()
    {
        _webDriverSetup = new WebDriverSetup();
        _webDriverSetup.SetupLocalProxyInstance();
    }

    [OneTimeTearDown]
    public void GlobalTearDown()
    {
        _webDriverSetup.TearDownLocalProxyInstance();
    }
}
