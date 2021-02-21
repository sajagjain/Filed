using AutoMapper;
using Filed.Models;
using Filed.Services.Contracts;
using Filed.Services.Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Filed.Controllers
{
    [Route("api")]
    [ApiController]
    public class FiledController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly IPaymentService _paymentService;

        public FiledController(IMapper mapper, IPaymentService paymentService)
        {
            this._mapper = mapper;
            this._paymentService = paymentService;
        }

        [Route("ProcessPayment")]
        [HttpPost]
        public StatusCodeResult ProcessPayment(PaymentVM payment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Convert Model to Actual Payment Model
                    Payment p = _mapper.Map<Payment>(payment);
                    _paymentService.ProcessPayment(p);

                    //Send when payment is processed
                    return Ok();
                }
                else
                {
                    //Send when bad request
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                //Send when Internal Server Error
                return new StatusCodeResult(500);
            }
        }
    }
}
