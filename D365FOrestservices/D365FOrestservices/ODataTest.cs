using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.OData.Client;
using D365FOrestservices.Microsoft.Dynamics.DataEntities;
using System.Collections;

namespace D365FOrestservices
{
    class ODataUserContract
    {
        public string userName;
        public string password;
        public string domain;
    }
    class ODataApplicationContract
    {
        public string applicationId;
        public string resource;
        public string result;
    }
    public class ODataRequestContract
    {
        public string company;
    }

    class ODataTest
    {
        public const string OAuthHeader = "Authorization";
        public ODataUserContract userContract;
        public ODataApplicationContract appContract;
        public ODataRequestContract request;
        string authenticationHeader;
        public string response;

        private AuthenticationResult GetAuthorization()
        {
            UriBuilder uri = new UriBuilder("https://login.windows.net/" + userContract.domain);
            AuthenticationContext authenticationContext
            = new AuthenticationContext(uri.ToString());
            UserPasswordCredential credential = new
            UserPasswordCredential(
            userContract.userName, userContract.password);
            Task<AuthenticationResult> task = authenticationContext.AcquireTokenAsync(
            appContract.resource,
            appContract.applicationId, credential);
            task.Wait();
            AuthenticationResult
            authenticationResult = task.Result;
            return authenticationResult;
        }

        public Boolean Authenticate()
        {
            AuthenticationResult authenticationResult;
            try
            {
                authenticationResult = GetAuthorization();
                //this gets the authorization token, this
                // must be set on the Http header for all requests
                authenticationHeader =
                authenticationResult.CreateAuthorizationHeader();
            }
            catch (Exception e)
            {
                response = "Authentication failed: " + e.Message;
                return false;
            }
            response = "OK";
            return true;
        }

        private Resources MakeResources()
        {
            string entityRootPath = appContract.resource + "/data";
            Uri oDataUri = new Uri(entityRootPath,
            UriKind.Absolute);
            var resources = new Resources(oDataUri);
            resources.SendingRequest2 += new
            EventHandler<SendingRequest2EventArgs>(
            delegate (object sender,
            SendingRequest2EventArgs e)
            {
                // This event handler is needed to set
                // the authentication code we got when
                // we logged on.
                e.RequestMessage.SetHeader(OAuthHeader,
                authenticationHeader);
            });
            return resources;
        }

        public System.Collections.ArrayList GetSalesQuotationNameList()
        {
            ArrayList array = new ArrayList();
            var resources = this.MakeResources();

            resources.SalesQuotationHeadersV2.AddQueryOption("DataAreaId", request.company);

            foreach (var obj in resources.SalesQuotationHeadersV2)
            {
                array.Add(obj.SalesQuotationName);
            }

            return array;
        }

        /*public Boolean UpdateVehicleNames()
        {
            var resources = this.MakeResources();
            resources.SalesQuotationHeadersV2.AddQueryOption("DataAreaId", request.company);
            foreach (var vehicle in resources.SalesQuotationHeadersV2)
            {
                vehicle.Description = vehicle.VehicleId
                + " : OData did it";
                resources.UpdateObject(vehicle);
            }
            try
            {
                resources.SaveChanges();
            }
            catch (Exception e)
            {
                response = e.InnerException.Message;
                return false;
            }
            return true;
        }*/

        /*public Boolean CreateNewVehicle(
        ConWHSVehicleTable _newVehicle)
        {
            var resources = this.MakeResources();
            _newVehicle.DataAreaId = request.company;
            resources.AddToConWHSVehicleTables(_newVehicle);
            try
            {
                resources.SaveChanges();
            }
            catch (Exception e)
            {
                response = e.InnerException.Message;
                return false;
            }
            return true;
        }*/
    }
}
