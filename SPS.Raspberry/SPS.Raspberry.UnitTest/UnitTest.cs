using System;
using SPS.Raspberry.UnitTest.Mock;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SPS.Raspberry.Core.MFRC522;

namespace SPS.Raspberry.UnitTest
{
    [TestClass]
    public class AuthorizationTest
    {
        [TestMethod]
        public void SEND_AUTH_REQUEST_EXPECTED_SUCCESS_RESPONSE()
        {
            MockMFRC522 mock = new MockMFRC522();

            mock.SetExptectedUid(new byte[] { 0, 1, 2, 3 });
        }
    }
}
