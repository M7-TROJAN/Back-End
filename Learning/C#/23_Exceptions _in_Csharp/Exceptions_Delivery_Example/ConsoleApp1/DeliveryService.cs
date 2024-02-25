using System;

namespace Exceptions.DeliveryExample
{
    public class DeliveryService
    {
        private readonly static Random random = new Random();

        public void Start(Delivery delivery)
        {
            try
            {
                Processing(delivery);

                Ship(delivery);

                Transit(delivery);

                Deliver(delivery);
            }
            catch (AccidentException ex)
            {
                throw;
                //Console.WriteLine($"There Was an Accident at {ex.Location} " + ex.Message);
                //delivery.Status = DeliveryStatus.UNKNOWN;
            }
            catch (Exception ex)
            {
                // choose What you want to make:
                // - inform the user
                // - log the exception 
                // - Ducking the exception (rethrowing) "متعملش اي حاجة وارميه للمستدعي"

                // whe choose Ducking the exception (rethrowing)
                throw;


                //Console.WriteLine("An error occurred due To: " + ex.Message);
                //delivery.Status = DeliveryStatus.UNKNOWN;
            }
        }

        // Process the delivery
        private void Processing(Delivery delivery)
        {
            // Simulate processing logic

            fakeIt("processing");
            if(random.Next(1,5) == 1) 
            {
                throw new InvalidOperationException("Unable To Procces the Item");
            }

            // When processing is complete, mark delivery status as PROCESSED
            delivery.Status = DeliveryStatus.PROCESSED;
        }

        // Ship the delivery
        private void Ship(Delivery delivery)
        {
            // Simulate shipping logic

            fakeIt("shipping");
            if (random.Next(1, 5) == 1)
            {
                throw new InvalidOperationException("Parcel is damged during the loading Process");
            }

            // When shipping is complete, mark delivery status as SHIPPED
            delivery.Status = DeliveryStatus.SHIPPED;
        }

        // Put the delivery in transit
        private void Transit(Delivery delivery)
        {
            // Simulate transit logic

            fakeIt("On It's Whay");

            if (random.Next(1, 5) == 1)
            {
                // assume you get the location from GPS
                string location = "365 ST Cairo Egypt";

                throw new AccidentException(location, "Ya Sater Ya Rab ACCIDENT !!!!");
            }

            // When the shipment is in transit, mark delivery status as IN TRANSIT
            delivery.Status = DeliveryStatus.INTRANSIT;
        }

        // Deliver the shipment
        private void Deliver(Delivery delivery)
        {
            // Simulate delivery logic

            fakeIt("Delivering");

            if (random.Next(1, 5) == 1)
            {
                throw new InvalidAddressException(delivery.Address, $"The Address '{delivery.Address}' Is Invalid !!!!");
            }

            // When delivery is complete, mark delivery status as DELIVERED
            delivery.Status = DeliveryStatus.DELIVERED;
        }

        private void fakeIt(string title)
        {
            Console.Write(title);
            for(int i = 0; i < 6; i++)
            {
                System.Threading.Thread.Sleep(300);
                Console.Write(".");
            }
            Console.WriteLine();
        }
    }
}
