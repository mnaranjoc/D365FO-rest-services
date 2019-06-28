using System;
using D365FOrestservices;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ODataApplicationContract appContract;
            appContract = new ODataApplicationContract();
            appContract.resource = "https://bioresearch-devdevaos.sandbox.ax.dynamics.com/";
            appContract.applicationId = "6ee49733-a2ed-47a2-8dd3-be070d0a38d6";
            ODataUserContract userContract = new
            ODataUserContract();
            Console.WriteLine("Use the O365 account that you use to log in to Dynamics 365 for Operations");
            Console.Write("O365 Username: ");
            userContract.userName = Console.ReadLine();
            Console.Write("O365 Password: ");
            userContract.password = Console.ReadLine();
            Console.WriteLine("This is your tenant, such as yourdomain.com or <yourtenant>.onmicrosoft.com");
            Console.Write("O365 Domain: ");
            userContract.domain = Console.ReadLine();
            ODataTest test = new ODataTest();
            test.userContract = userContract;
            test.appContract = appContract;
            if (!test.Authenticate())
            {
                Console.WriteLine(test.response);
            }
            test.request = new ODataRequestContract();
            test.request.company = "USMF";
            System.Collections.ArrayList vehicleNames = test.GetSalesQuotationNameList();
            foreach (var vehicleName in vehicleNames)
            {
                Console.WriteLine(vehicleName);
            }
            Console.WriteLine("Changing vehicle descriptions");
            test.GetSalesQuotationNameList();
            /*SalesQuota vehicle = new ConWHSVehicleTable();
            Console.WriteLine("Create a new Vehicle");
            Console.Write("Vehicle Id: ");
            vehicle.VehicleId = Console.ReadLine();
            Console.Write("Vehicle group: ");
            vehicle.VehicleGroupId = Console.ReadLine();
            Console.Write("Description: ");
            vehicle.Description = Console.ReadLine();
            test.CreateNewVehicle(vehicle);
            Console.WriteLine("Press enter to continue.");*/
            Console.ReadLine();
        }
    }
}