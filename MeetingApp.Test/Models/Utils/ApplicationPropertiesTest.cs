using MeetingApp.Utils;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace MeetingApp.Test.Models.Utils
{
    public class ApplicationPropertiesTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// 正常系
        /// </summary>
        [Test]
        public void ApplicationProperties_SaveToProperties_Success()
        {
            MockForms.Init();
            Application.Current = new CustomApplication();

            var applicationProperties = new ApplicationProperties();
            applicationProperties.SaveToProperties<string>("testSaveKey", "testSaveValue");

            Assert.IsTrue("testSaveValue".Equals(applicationProperties.GetFromProperties<string>("testSaveKey")));
        }
        [Test]
        public void ApplicationProperties_GetFromProperties_Success()
        {
            MockForms.Init();
            Application.Current = new CustomApplication();
            Application.Current.Properties.Add("testSuccessKey", "testValue");

            var applicationProperties = new ApplicationProperties();

            Assert.IsTrue("testValue".Equals(applicationProperties.GetFromProperties<string>("testSuccessKey")));
        }
        [Test]
        public void ApplicationProperties_ClearPropertie_Success()
        {
            MockForms.Init();
            Application.Current = new CustomApplication();
            Application.Current.Properties.Add("testClearKey", "testClearValue");

            var applicationProperties = new ApplicationProperties();
            applicationProperties.ClearPropertie("testClearKey");

            Assert.IsNull(applicationProperties.GetFromProperties<string>("testClearKey"));
        }

        /// <summary>
        /// 異常系
        /// </summary>
        [Test]
        public void ApplicationProperties_GetFromProperties_Failure()
        {
            MockForms.Init();
            Application.Current = new CustomApplication();
            Application.Current.Properties.Add("testKey", "testValue");

            var applicationProperties = new ApplicationProperties();

            Assert.IsNull(applicationProperties.GetFromProperties<string>(string.Empty));
            Assert.IsNull(applicationProperties.GetFromProperties<string>("notExistKey"));
        }


        private class CustomApplication : Application
        {
            public CustomApplication()
            {
                Resources = new ResourceDictionary();
                Resources["Chuck"] = "Norris";
                MainPage = new ContentPage();
            }
        }
    }
}
