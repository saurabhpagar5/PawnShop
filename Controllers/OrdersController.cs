using Microsoft.AspNetCore.Mvc;
using PawnShop.Models;
using Razorpay.Api;

namespace PawnShop.Controllers
{
    public class OrdersController : Controller
    {
        [BindProperty]
        public OrderEntity _OrderDetails { get; set; }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InitiateOrder()
        {

            Random _random = new Random();
            string TransactionId = _random.Next(0,10000).ToString();


            Dictionary<string, object> input = new Dictionary<string, object>();

            input.Add("amount", Convert.ToDecimal(_OrderDetails.TotalAmount)*100); // this amount should be same as transaction amount
            input.Add("currency", "INR");
            input.Add("receipt", TransactionId);

            string key = "rzp_test_J5kmJQlR2TPgIl";
            string secret = "pl96zXVB197CRoZj4jaUQs5m";

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            ViewBag.orderid = order["id"].ToString();

            return View("Payment", _OrderDetails);

        }

        public IActionResult Payment(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("razorpay_payment_id", razorpay_payment_id);
            attributes.Add("razorpay_order_id", razorpay_order_id);
            attributes.Add("razorpay_signature", razorpay_signature);

            Utils.verifyPaymentSignature(attributes);

            OrderEntity orderdetails = new OrderEntity();

            orderdetails.TransactionId = razorpay_payment_id;
            orderdetails.OrderId = razorpay_order_id;

            return View("PaymentSuccess", orderdetails);


        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}
