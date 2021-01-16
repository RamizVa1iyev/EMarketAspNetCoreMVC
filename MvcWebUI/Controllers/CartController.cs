using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using MvcWebUI.Helpers;
using MvcWebUI.Models;

namespace MvcWebUI.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private ICartSessionHelper _cartSessionHelper;
        private IProductService _productService;

        public CartController(ICartService cartService, IProductService productService, ICartSessionHelper cartSessionHelper)
        {
            _cartService = cartService;
            _productService = productService;
            _cartSessionHelper = cartSessionHelper;
        }

        public IActionResult AddToCart(int productId)
        {
            var product = _productService.GetById(productId);
            var cart = _cartSessionHelper.GetCart("cart"); 
            _cartService.AddToCart(cart,product);

            _cartSessionHelper.SetCart("cart",cart);
            TempData.Add("message",product.ProductName+" added to cart!");
            return RedirectToAction("Index","Product");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var product = _productService.GetById(productId);

            var cart = _cartSessionHelper.GetCart("cart");
            _cartService.RemoveFromCart(cart, productId);

            _cartSessionHelper.SetCart("cart", cart);
            TempData.Add("message", product.ProductName + " deleted from cart!");

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Index()
        {
            var model = new CartListViewModel()
            {
                Cart=_cartSessionHelper.GetCart("cart")
            };

            return View(model);
        }

        public IActionResult Complete()
        {
            var model = new ShippingDetailsViewModel()
            {
                ShippingDetail =new ShippingDetail()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Complete(ShippingDetail shippingDetail)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            TempData.Add("message", "your order has been completed");
            _cartSessionHelper.Clear();
            return RedirectToAction("Index", "Product");
        }
    }
}
