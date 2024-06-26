using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using Stripe.Checkout;


namespace NewStripe.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    
    [Route("create-checkout-session")]
    [HttpPost]
    public async Task<ActionResult> Create(string priceId)
    {
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1,
                },
            },
            Mode = "subscription",
            SuccessUrl = "https://zeuxinnovation.com/wp-content/uploads/2023/04/maximising-user-satisfaction-1.jpg",
            CancelUrl = "https://cdn-icons-png.flaticon.com/512/6659/6659895.png",
        };
        var service = new SessionService();
        Session session = service.Create(options);

        return Ok(new JsonResult(new
        {
            url = session.Url,
            currency = session.Currency
        }));
    }
    
    [HttpPost("webhook")]
    public async Task<IActionResult> Hook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                //var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                //Console.WriteLine(new JsonResult(paymentIntent).Value);
                Console.WriteLine("Payment intent succeeded");
            }
            else if (stripeEvent.Type == Events.ProductCreated)
            {
                Console.WriteLine("Product created");
            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }
}