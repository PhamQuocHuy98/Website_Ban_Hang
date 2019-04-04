using Do_An_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;
using PayPal.Api;

namespace Do_An_Web.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
        public ActionResult XemGioHang()
        {

            var laygiohang = LayGioHang();
            return View(laygiohang);
        }
        public List<ItemGioHang> LayGioHang()
        {
            // Kiểm tra xem giỏ hàng tồn tại chưa
            List<ItemGioHang> lstItemGioHang= Session["GioHang"] as List<ItemGioHang>;
            // nếu chưa tồn tại
            if (lstItemGioHang == null)
            {
                lstItemGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstItemGioHang;
            }
            return lstItemGioHang;
        }

        public ActionResult ThemGioHang(int MaSP)
        {
            // kiểm tra xem dưới csdl có sản phẩm có masp hợp lệ hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                return HttpNotFound();
            }
            List < ItemGioHang > lstItemGioHang = LayGioHang();
            // nếu 1 sản phẩm đã có trong giỏ hàng thì tăng số lượng 
            ItemGioHang item = lstItemGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(item!=null)
            {
                // nếu người mua nhìu hơn số lượng tồn
                if(item.SoLuong>sp.SoLuongTon)
                {
                    return Content( " <script> alert(\"Sản phẩm đã hết hàng! \")</script> ");
                }
                item.SoLuong++;
                item.ThanhTien = item.SoLuong * item.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                ViewBag.lgh = LayGioHang();
                return PartialView("GioHangPartial");
            }
            
           

            // tạo ra 1 item giỏ hàng sau đó add vô list
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (itemGH.SoLuong > sp.SoLuongTon)
            {
                return Content(" <script>alert(\"Sản phẩm đã hết hàng! \")</script> ") ; // return thông báo số lượng k đủ 
            }
            lstItemGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();

            return PartialView("GioHangPartial");
        }

        public int TinhTongSoLuong()
        {
            List<ItemGioHang> lstItemGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstItemGioHang == null)
            {
                return 0;
            }
            return lstItemGioHang.Sum(n => n.SoLuong);
        }
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstItemGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstItemGioHang == null)
            {
                return 0;
            }
            return lstItemGioHang.Sum(n => n.ThanhTien);
        }
        public ActionResult GioHangPartial()
        {
            List<ItemGioHang> LGH = LayGioHang();
            ViewBag.check = 1;
            if (LGH == null)
            {
                ViewBag.check = 0;
            }
            if (TinhTongSoLuong()==0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                ViewBag.lgh = LGH;
                return PartialView();
            }
               
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            ViewBag.lgh = LGH;
            return PartialView(LGH);
        }

        [HttpGet]
        public ActionResult CapNhatGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiểm tra sp có trong csdl không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                return HttpNotFound();
            }
            List<ItemGioHang> lstGioHang = LayGioHang();

            // kiểm tra sản phẩm có tồn tại trong giỏ hàng không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.GioHang = lstGioHang;
            if (ViewBag.GioHang == null)
            {
                return HttpNotFound();
            }
         
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang item)
        {
            
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == item.MaSP);
            if (sp.SoLuongTon < item.SoLuong)
            {
                return null;
            }
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang Itemmoi = lstGH.Find(n => n.MaSP == item.MaSP);
            Itemmoi.SoLuong = item.SoLuong;
            Itemmoi.ThanhTien = Itemmoi.SoLuong * Itemmoi.DonGia;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int MaSP)
        {

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiểm tra sp có trong csdl không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                return HttpNotFound();
            }

            var lstGH = LayGioHang();

            ItemGioHang item = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            if (item == null)
            {
                return RedirectToAction ("Home", "Index");
            }
            lstGH.Remove(item);

            return RedirectToAction("XemGioHang");

        }
        public ActionResult DatHang()
        {
            ViewBag.TongTien = TinhTongTien();
           
            var laygiohang = LayGioHang();
            return View(laygiohang);
        }
        public ActionResult ThanhToan()
        {

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Home", "Index");
            }
            if (Session["TaiKhoan"] == null)
            {
                //return View()
                return RedirectToAction("DangNhapMuaHang", "Home");
            }
            
            //Thêm đơn đặt hàng
            DonDatHang ddh = new DonDatHang();
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            ddh.MaKH = kh.MaKH;
            db.DonDatHangs.Add(ddh);
            
            db.SaveChanges();
            // Thêm chi tiết đơn đặt hàng
            
            List<ItemGioHang> lst = LayGioHang();
            foreach(var item in lst)
            {
                ChiTietDonDatHang ctddh = new ChiTietDonDatHang();
                ctddh.MaDDH = ddh.MaDDH;
                ctddh.MaSP = item.MaSP;

                ctddh.SoLuong = item.SoLuong;
                ctddh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctddh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;

            //return View();
            return RedirectToAction("Index", "Home");
        }
        public decimal ChuyenTienVietSangTienDo(decimal? tien)
        {
            //20000 = 0.86$ 
            //tien = ?
            double?tientemp = (double?)tien;
            decimal ketqua = (decimal)((tientemp * 0.86) / 20000);
            return ketqua;
        }
        //------------------Paypal
        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/GioHang/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
               
                return View("FailureView");
            }

            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
       
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            
            
            var itemList = new ItemList() { items = new List<Item>() };

            List<ItemGioHang> lstGioHang = LayGioHang();

            foreach(var giohang in lstGioHang)
            {
                itemList.items.Add(new Item()
                {

                    name = giohang.TenSP,
                    currency = "USD",
                    price = ChuyenTienVietSangTienDo(giohang.DonGia).ToString(),
                    quantity = giohang.SoLuong.ToString(),
                    sku = "sku"
                });
            }
            
            

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = ChuyenTienVietSangTienDo(lstGioHang.Sum(n => n.DonGia * n.SoLuong)).ToString()
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax )+ Convert.ToDouble(details.shipping)+ Convert.ToDouble(details.subtotal)).ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Thanh Toán ",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }
    }
}