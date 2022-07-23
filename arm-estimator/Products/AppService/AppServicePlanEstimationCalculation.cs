internal class AppServicePlanEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AppServicePlanEstimationCalculation(RetailItem[] items)
        : base(items)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.Where(_ => _.type != "Reservation" && _.type != "DevTestConsumption")
            .OrderByDescending(_ => _.retailPrice);
    }

    // TODO: Functions Premium plan calculation should include multiplication if used EP2 / EP3
    public double GetTotalCost()
    {
        double? estimatedCost = 0;
        var items = GetItems();

        foreach(var item in items)
        {
            // vCPU Duration
            if (item.meterId == "2099ccfe-9c25-4ae2-9e35-6500db3b8e74")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Memory Duration
            else if (item.meterId == "8fca9dc0-1307-4b8c-986c-d58505cf1e4b")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // D1
            else if (item.meterId == "0c3885bd-351d-4c28-830a-446d7bb4295c")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // B1 Windows
            else if (item.meterId == "ba302f7a-078b-4141-a636-a76315ba44ce")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // B1 Linux
            else if (item.meterId == "e08d4be1-d8c1-49ba-a814-6369d6f14c34")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // B2 Windows
            else if (item.meterId == "4f1fa8fe-ffed-48fa-9969-26a150775b60")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // B2 Linux
            else if (item.meterId == "953d62f6-b048-4311-8ea6-a4ccf5a50e5d")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // B3 Windows
            else if (item.meterId == "7ce53686-7c67-470b-ae6f-3996cbb02fbb")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // B3 Linux
            else if (item.meterId == "f8dbccfa-422e-4b48-83d2-234ec8ad6fbb")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // S1 Windows
            else if (item.meterId == "505db374-df8a-44df-9d8c-13c14b61dee1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // S1 Linux
            else if (item.meterId == "2af3b3dd-b3a9-4117-b47f-1da3a2a225ed")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // S2 Windows
            else if (item.meterId == "64d48263-32ab-4359-b05b-8626b0974e17")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // S2 Linux
            else if (item.meterId == "c1489f84-52ff-4804-983e-79e89dad4f4c")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // S3 Windows
            else if (item.meterId == "7fadaaec-3afd-4afb-89a7-cdb1e3cb702b")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // S3 Linux
            else if (item.meterId == "5a4110a6-5f99-4f39-a284-aceedabf049a")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P1
            else if (item.meterId == "db2ae25a-4469-4fa3-9c27-63ac21da6b30")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P2
            else if (item.meterId == "7a3b96e3-1c68-4506-86d8-224b89b3347f")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P3
            else if (item.meterId == "ef5d838f-bed0-4e72-8fb2-ec32d6a715f1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P1V2 Windows
            else if (item.meterId == "e2639bb9-476a-45c7-b87a-9ffa79237c29")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P1V2 Linux
            else if (item.meterId == "de19d66e-b6d4-421b-b9c7-270b1ff7fea8")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P2V2 Windows
            else if (item.meterId == "7d063dd3-cf66-4699-980d-2f557f89f0fb")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P2V2 Linux
            else if (item.meterId == "c77c8059-8725-4f1c-a6ce-947172332cc4")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P3V2 Windows
            else if (item.meterId == "5de30724-60b0-40c2-b2a0-2235227991d4")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P3V2 Linux
            else if (item.meterId == "0c9ab313-3f9c-4763-b0cd-b9671345c98b")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P1V3 Windows
            else if (item.meterId == "ddd10d22-1c66-5413-8aa4-00f92d96cf37")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P1V3 Linux
            else if (item.meterId == "cc659186-8327-58c4-beb4-1900b2b64af2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P2V3 Windows
            else if (item.meterId == "601ac801-9e1a-5e1f-865d-d4a90145eb51")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P2V3 Linux
            else if (item.meterId == "0cd29c4f-1213-56ad-ba23-7c760f10aab9")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P3V3 Windows
            else if (item.meterId == "4d89b7e8-b5b4-5f6c-a503-d8aa3a42dac2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // P3V3 Linux
            else if (item.meterId == "6fb105a4-26ad-554e-8205-81b103701705")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
