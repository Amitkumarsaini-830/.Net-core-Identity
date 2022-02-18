using com.aadviktech.IMS.Constant;
using com.aadviktech.IMS.Constant.AllEnum;
using com.aadviktech.IMS.Contract.UOW_Interfaces;
using com.aadviktech.IMS.DB;
using com.aadviktech.IMS.ViewModel;
using com.aadviktech.IMS.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace com.aadviktech.IMS.web.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private IUserUow UserUowObj;
        private IItemUOw ItemUOw;
        JsonSerializerSettings _jsonSeriesSetting = new JsonSerializerSettings();
        private IItemUOw itemUOw;
        private ITransactionsUOW transactionsUOW;
        private ILedgerUOW ledgerUOW;


        public ClientController(IUserUow UserUowObj, IItemUOw ItemUOw, IItemUOw itemUOw, ITransactionsUOW transactionsUOW,ILedgerUOW ledgerUOW)
        {
            this.UserUowObj = UserUowObj;
            this.ItemUOw = ItemUOw;
            _jsonSeriesSetting.ContractResolver = new DefaultContractResolver();
            this.itemUOw = itemUOw;
            this.transactionsUOW = transactionsUOW;
            this.ledgerUOW = ledgerUOW;
        }
        public async Task<IActionResult> Index()
        {
            int UserId = await SessionUtility.GetUserId();
            List<Transaction> transactions = transactionsUOW.AllTransactions(UserId);
            // ViewBag.Expenses = itemUOw.Expenses(UserId);
            return View(transactions);
        }


        public async Task<IActionResult> UserProfile()
        {
            ViewBag.AccountantList = UserUowObj.GetAccountants();
            ViewBag.states = UserUowObj.GetState();
            int UserId = await SessionUtility.GetUserId();
            MyAppUser myAppUser = UserUowObj.GetById(UserId);
            return View(myAppUser);
        }
        [HttpPost]
        public async Task<IActionResult> UserProfile(MyAppUser user)
        {

            if (ModelState.IsValid)
            {
                Dictionary<string, object> dict = await UserUowObj.EditClient(user);
                if (Convert.ToBoolean(dict["Status"]))
                {
                    ModelState.Clear();
                    ViewBag.msg = Convert.ToString(dict["Message"]);
                }
                else
                {
                    ViewBag.error = Convert.ToString(dict["Message"]);
                }
            }
            //MyAppUser user1 = UserUowObj.GetById(user.Id);
            return RedirectToAction("Index");
        }

        // Ledger Group

        public async Task<IActionResult> LedgerGroup()
        {
            int UserId = await SessionUtility.GetUserId();
            List<LedgerGroup> ledgerGroups = UserUowObj.ledgerGroups(UserId);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(ledgerGroups);
        }

        [HttpPost]
        public async Task<IActionResult> AddLedgerGroup(LedgerGroup ledger)
        {
            ledger.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddLedgerGroup(ledger);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("LedgerGroup");
        }

        public IActionResult DeleteLedgerGroup(int id)
        {
            return Json(UserUowObj.DeleteLedgerGroup(id));
        }

        public IActionResult GetEditLedgerGroup(int id)
        {
            return Json(UserUowObj.GetLedger(id));
        }


        [HttpPost]
        public async Task<IActionResult> EditLedgerGroup(LedgerGroup ledger)
        {
            ledger.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.EditLedgerGroup(ledger);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("LedgerGroup");
        }

        // Ledger Sub Group


        public async Task<IActionResult> GetLedgerSubGroup(int Id)
        {
            int UserId = await SessionUtility.GetUserId();
            List<LedgerSubGroup> ledgerSubs = UserUowObj.ledgerSubGroups(UserId).Where(a => a.ledgerCategoryId == Id).ToList();
            return Json(ledgerSubs);
        }
        public async Task<IActionResult> LedgerSubGroup()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.ledgerGroups = UserUowObj.ledgerGroups(UserId);
            List<LedgerSubGroup> ledgerSubGroups = UserUowObj.ledgerSubGroups(UserId);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(ledgerSubGroups);
        }

        [HttpPost]
        public async Task<IActionResult> AddLedgerSubGroup(LedgerSubGroup ledgerSub)
        {
            ledgerSub.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddLedgerSubGroup(ledgerSub);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("LedgerSubGroup");
        }

        public IActionResult DeleteLedgerSubGroup(int id)
        {
            return Json(UserUowObj.DeleteLedgerSubGroup(id));
        }

        public IActionResult GetEditLedgerSubGroup(int id)
        {
            return Json(UserUowObj.GetSubLedger(id));
        }


        [HttpPost]
        public async Task<IActionResult> EditLedgerSubGroup(LedgerSubGroup ledger)
        {
            ledger.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.EditLedgerSubGroup(ledger);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("LedgerSubGroup");
        }



        public IActionResult Reposrts()
        {
            return View();
        }

        public async Task<IActionResult> AddPatry()
        {
            int UserId = await SessionUtility.GetUserId();
            //  List<MyAppUser> CleienList = UserUowObj.GetClient();
            List<State> states = UserUowObj.GetState();
            List<LedgerSubGroup> ledgers = UserUowObj.ledgerSubGroups(UserId);
            ViewBag.states = states;
            ViewBag.ledgers = ledgers;
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(ledgers);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatry(MyAppUser User)
        {
            // int UserId = await SessionUtility.GetUserId();
            User.ClientId = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddParty(User);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("PartyTransactions");
        }

        public async Task<IActionResult> GetCityList(int StateId)
        {
            List<City> cities = UserUowObj.GetCity(StateId);
            return Json(cities);
        }


        public IActionResult PartyTransactions()
        {
            return View();
        }

        public IActionResult PartyList()
        {
            return View();
        }

        public IActionResult PartyListTrnx()
        {
            return View();
        }
        public async Task<IActionResult> ClientTrnx(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Sp_ClientTransaction> ModelList = UserUowObj.GetClienTransactions(TablePaging);

            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }

        public IActionResult ClientDetals(int id)
        {
            MyAppUser myApp = UserUowObj.GetById(id);
            return Json(myApp);
        }

        public IActionResult GetClientTarnsaction(int Id, string name, string From, string To, string Invoice, string Amount, int AseDec)
        {
            return View(transactionsUOW.ClentTransactonts(Id, name, From, To, Invoice, Amount, AseDec));
        }

        public async Task<IActionResult> ClientGrid(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<MyAppUser> ModelList = UserUowObj.GetPartyList(ref TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }

        public async Task<IActionResult> EditParty(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            MyAppUser user = UserUowObj.GetById(id);
            ViewBag.PartyList = UserUowObj.GetClient();
            ViewBag.states = UserUowObj.GetState();
            ViewBag.Ledger = UserUowObj.ledgerSubGroups(UserId);
            if (user != null)
            {
                return View(user);
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditParty(MyAppUser user)
        {

            if (ModelState.IsValid)
            {
                Dictionary<string, object> dict = await UserUowObj.EditParty(user);
                if (Convert.ToBoolean(dict["Status"]))
                {
                    ModelState.Clear();
                    ViewBag.msg = Convert.ToString(dict["Message"]);
                }
                else
                {
                    ViewBag.error = Convert.ToString(dict["Message"]);
                }
            }
            //MyAppUser user1 = UserUowObj.GetById(user.Id);
            return RedirectToAction("PartyTransactions");
        }

        public async Task<IActionResult> Tax()
        {
            int UserId = await SessionUtility.GetUserId();
            List<TaxRate> ModelList = UserUowObj.GetTaxList(UserId);

            return View(ModelList);
        }

        public async Task<IActionResult> TaxList()
        {
            int UserId = await SessionUtility.GetUserId();
            List<TaxRate> ModelList = UserUowObj.GetTaxList(UserId);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            // ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);

        }



        [HttpPost]
        public async Task<IActionResult> AddTaxRate(TaxRate tax)
        {

            tax.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.ReateTaxRate(tax);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("Tax");

        }

        [HttpGet]
        public IActionResult GetTaxRate(int id)
        {
            TaxRate TaxRate = UserUowObj.GetTaxById(id);

            return Json(TaxRate);
        }


        [HttpPost]
        public async Task<IActionResult> EditTaxRate(TaxRate tax)
        {

            Dictionary<string, object> dict = await UserUowObj.EditTaxRate(tax);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("Tax");

        }

        public IActionResult DeleteTax(int id)
        {
            return Json(UserUowObj.DeleteTaxRate(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddItemCategories(ItemCategory itemCategory)
        {
            itemCategory.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.AddItemCategoty(itemCategory);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("ItemCategoryList");
        }

        public IActionResult DeleteItemCategory(int id)
        {
            return Json(itemUOw.DeleteItemCategoty(id));
        }

        public IActionResult EditItemCategory(int id)
        {
            return Json(itemUOw.GetItemCategory(id));
        }

        [HttpPost]
        public async Task<IActionResult> EditItemCategories(ItemCategory itemCategory)
        {
            itemCategory.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.EditItemCategoty(itemCategory);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("ItemCategoryList");
        }

        public async Task<IActionResult> ItemCategoryList()
        {
            int UserId = await SessionUtility.GetUserId();
            List<ItemCategory> itemCategory = ItemUOw.ItemCategoryList(UserId).ToList();
            return View(itemCategory);
        }

        public async Task<IActionResult> ItmCategoryList(int CategotyId)
        {
            int UserId = await SessionUtility.GetUserId();
            List<ItemSubCategory> subCategories = ItemUOw.ItemCategorySubList(UserId).Where(a => a.Categoryid == CategotyId).ToList();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return Json(subCategories);
        }

        public async Task<IActionResult> ItemSubCategory()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.ItemCatList = itemUOw.ItemCategoryList(UserId).ToList(); ;
            List<ItemSubCategory> subCategories = itemUOw.ItemCategorySubList(UserId).ToList();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }

            return View(subCategories);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemSubCategory(ItemSubCategory itemCategory)
        {
            itemCategory.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.AddItemSubCategoty(itemCategory);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("ItemSubCategory");
        }

        public IActionResult DeleteItemSubCategory(int id)
        {
            return Json(itemUOw.DeleteItemSubCategoty(id));
        }

        public IActionResult EditItemSubCategory(int id)
        {
            return Json(itemUOw.GetSubCategory(id));
        }


        [HttpPost]
        public async Task<IActionResult> EditItemSubCategories(ItemSubCategory itemCategory)
        {
            itemCategory.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.EditItemSubCategoty(itemCategory);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("ItemSubCategory");
        }






        public async Task<IActionResult> TaxGrp()
        {
            int UserId = await SessionUtility.GetUserId();
            List<TaxGroup> ModelList = UserUowObj.GetTaxIdsforGrp(UserId);
            ViewBag.TaxRate = UserUowObj.GetTaxList(UserId);

            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");

            }

            //ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);

        }

        [HttpPost]
        public async Task<IActionResult> AddTaxGrp(string Name, List<int> TaxId)
        {
            TaxGroup taxGroup = new TaxGroup();
            taxGroup.CreatedBy = await SessionUtility.GetUserId();
            taxGroup.Name = Name;
            string str = String.Join(",", TaxId);
            taxGroup.TaxRateIds = str;


            Dictionary<string, object> dict = await UserUowObj.AddTaxgrp(taxGroup);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("Tax");

        }

        [HttpGet]
        public IActionResult EditTaxGrp(int id)
        {
            TaxGroup TaxGrp = UserUowObj.GetTaxgrpById(id);

            return Json(TaxGrp);
        }
        [HttpPost]
        public async Task<IActionResult> EditTaxGrp(TaxGroup Grp, List<int> TaxId)
        {

            Dictionary<string, object> dict = await UserUowObj.EditTaxGrp(Grp, TaxId);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("Tax");

        }


        public IActionResult DeleteTaxGrp(int id)
        {
            return Json(UserUowObj.DeleteTaxGrp(id));
        }


        public async Task<IActionResult> Units()
        {
            int UserId = await SessionUtility.GetUserId();
            List<UnitName> unitNames = UserUowObj.GetUnitList(UserId).ToList();

            return View(unitNames);
        }

        public async Task<IActionResult> AddUnit(UnitName unit)
        {

            unit.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddUnit(unit);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("Units");

        }

        [HttpGet]
        public IActionResult EditUnit(int id)
        {
            UnitName unit = UserUowObj.GetUnitById(id);

            return Json(unit);
        }
        [HttpPost]
        public async Task<IActionResult> EditUnit(UnitName unit)
        {

            Dictionary<string, object> dict = await UserUowObj.EditUnit(unit);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("Units");

        }
        public IActionResult DeleteUnit(int id)
        {
            return Json(UserUowObj.DeleteUnit(id));
        }


        public async Task<IActionResult> ItemCategory()
        {
            int UserId = await SessionUtility.GetUserId();
            List<Item> categories = itemUOw.ItemEcpensesList(UserId);
            if (categories.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            ViewBag.TaxRate = UserUowObj.GetTaxList(UserId);
            return View(categories);

        }

        [HttpPost]
        public async Task<IActionResult> AddIncomeItem(Item item)
        {
            item.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.AddItem(item);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("IncomeCategory");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
                return RedirectToAction("IncomeCategory");

            }

        }



        [HttpPost]
        public async Task<IActionResult> AddItemCategory(Category category)
        {
            category.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.AddItemExpenseCategoty(category);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("ItemCategory");

            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
                return RedirectToAction("ItemCategory");

            }

        }

        public IActionResult DeleteCategory(int id)
        {
            return Json(itemUOw.DeleteExpenseCategoty(id));
        }

        // Income 



        public async Task<IActionResult> Income()
        {
            int UserId = await SessionUtility.GetUserId();
            // ViewBag.expenses = UserUowObj.Income();
            // ViewBag.ExpCatg = itemUOw.IncomeCategories(UserId);
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            ViewBag.states = UserUowObj.GetState();
            ViewBag.City = UserUowObj.GetCityList();
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.Shop = UserUowObj.GetShopes(UserId);
            ViewBag.ExtraCharge = UserUowObj.ExtraChargeList(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            if (payemtMode != null)
            {
                List<string> banks = new List<string>();
                if (payemtMode.IsBank == true)
                {
                    banks.Add(PaymentType.Bank.ToString());
                }
                if (payemtMode.IsCash == true)
                {
                    banks.Add(PaymentType.Cash.ToString());
                }
                if (payemtMode.IsWallet)
                {
                    banks.Add(PaymentType.Wallet.ToString());
                }
                if (payemtMode.IsFinance)
                {
                    banks.Add(PaymentType.Finance.ToString());
                }
                ViewBag.Transaction = banks;
            }
            else
            {
                ViewBag.Transaction = null;
            }
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            //  List<string> Desc=
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.Income);

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Income(Transaction income)
        {
            income.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await transactionsUOW.Transaction(income);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("IncomeList");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
                return RedirectToAction("IncomeList");
            }
        }
        public async Task<IActionResult> IncomeItemGrid(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Item> items = ItemUOw.GetItem(ref TablePaging);
            if (items.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(items);
        }
        public async Task<IActionResult> GetIncomeItem(int id)
        {
            ViewBag.indx = id;
            int UserId = await SessionUtility.GetUserId();
            ViewBag.ItemList = itemUOw.IncomeItemList(UserId).ToList();
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            return View();
        }
        public async Task<IActionResult> IncomeList()
        {
            int UserId = await SessionUtility.GetUserId();
            //  List<Transaction> transactions = transactionsUOW.Transaction(UserId);
            return View();
        }
        public async Task<IActionResult> IncomeGrid(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Transaction> ModelList = transactionsUOW.GetIncome(ref TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }

        public async Task<IActionResult> GetIncomeItemInfo(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            Item item = itemUOw.ItemGetById(id);
            return Json(item);
        }
        public async Task<IActionResult> IncomeItemInfo(int ItemIds)
        {
            int UserId = await SessionUtility.GetUserId();
            Item item = itemUOw.ItemGetById(ItemIds);
            return Json(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditIncomeItem(Item Itm)
        {
            Dictionary<string, object> dict = await ItemUOw.EditItem(Itm);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
            }

            return RedirectToAction("IncomeCategory");

        }
        public IActionResult DeleteIncomeItem(int id)
        {
            return Json(itemUOw.DeleteItem(id));
        }


        public async Task<IActionResult> IncomeItmCategoryList(int CategotyId)
        {
            int UserId = await SessionUtility.GetUserId();
            List<ItemSubCategory> subCategories = ItemUOw.IncomeCategorySubList(UserId).Where(a => a.Categoryid == CategotyId).ToList();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return Json(subCategories);
        }
        public async Task<IActionResult> IncomeCategory()
        {
            int UserId = await SessionUtility.GetUserId();
            List<Item> categories = itemUOw.IncomeItemList(UserId).ToList();
            if (categories.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.TaxRate = UserUowObj.GetTaxList(UserId);
            ViewBag.Category = itemUOw.IncomeCategoryList(UserId).ToList();
            ViewBag.SubCat = ItemUOw.IncomeCategorySubList(UserId).ToList(); ;

            return View(categories);

        }

        public async Task<IActionResult> IncomeCategorytype()
        {
            int UserId = await SessionUtility.GetUserId();
            List<ItemCategory> expense = itemUOw.IncomeCategoryList(UserId).ToList();
            ViewBag.LegderGrp = UserUowObj.ledgerGroups(UserId);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(expense);

        }
        [HttpPost]
        public async Task<IActionResult> IncomeCategorytype(ItemCategory income)
        {
            income.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await ItemUOw.AddItemCategoty(income);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("IncomeCategorytype");

            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
                return RedirectToAction("IncomeCategorytype");
            }
        }

        public async Task<IActionResult> EditIncomeCategorytype(ItemCategory income)
        {
            income.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await ItemUOw.EditItemCategoty(income);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("IncomeCategorytype");

            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
                return RedirectToAction("IncomeCategorytype");
            }
        }
        public IActionResult deleteIncomeCateGory(int id)
        {
            return Json(ItemUOw.DeleteItemCategoty(id));
        }

        public IActionResult EditIncomeCatItem(int id)
        {
            Item item = ItemUOw.ItemGetById(id);
            return Json(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditIncomeCatItem(Item Itm)
        {

            Dictionary<string, object> dict = await ItemUOw.EditItem(Itm);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
            }
            return RedirectToAction("IncomeCategory");

        }
        public async Task<IActionResult> IncomeSubCategory()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.IncomeCatecogy = ItemUOw.IncomeCategoryList(UserId).ToList();
            List<ItemSubCategory> incomeSubs = ItemUOw.IncomeCategorySubList(UserId).ToList();
            return View(incomeSubs);
        }
        [HttpPost]
        public async Task<IActionResult> IncomeSubCategory(ItemSubCategory incomeSub)
        {
            incomeSub.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await ItemUOw.AddItemSubCategoty(incomeSub);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("IncomeSubCategory");

            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
                return RedirectToAction("IncomeSubCategory");
            }
        }
        public async Task<IActionResult> GetIncomeSubCatItem(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            ItemSubCategory item = ItemUOw.GetSubCategory(id);
            return Json(item);
        }
        [HttpPost]
        public async Task<IActionResult> EditIncomeSubCatItem(ItemSubCategory Itm)
        {

            Dictionary<string, object> dict = await ItemUOw.EditItemSubCategoty(Itm);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
            }
            return RedirectToAction("IncomeSubCategory");

        }

        public IActionResult deleteIncomeSubCateGory(int id)
        {
            return Json(ItemUOw.DeleteItemSubCategoty(id));
        }

        public async Task<IActionResult> PaymentInIncome()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            List<string> banks = new List<string>();
            if (payemtMode.IsBank == true)
            {
                banks.Add(PaymentType.Bank.ToString());
            }
            if (payemtMode.IsCash == true)
            {
                banks.Add(PaymentType.Cash.ToString());
            }
            if (payemtMode.IsWallet)
            {
                banks.Add(PaymentType.Wallet.ToString());
            }
            if (payemtMode.IsFinance)
            {
                banks.Add(PaymentType.Finance.ToString());
            }
            ViewBag.Transaction = banks;
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.PaymentInIncome);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        public IActionResult PaymentInIncomeGrid(int Id)
        {
            List<Transaction> ModelList = transactionsUOW.GetIncomeDueAmt(Id);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            return View(ModelList);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentInIncome(Transaction transaction)
        {
            transaction.CreatedBy = await SessionUtility.GetUserId();
            transaction.TotalAmount = transaction.RecievedAmt;
            Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("PaymentInIncome");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("PaymentInIncome");
            }
        }


        public async Task<IActionResult> Item()
        {
            int UserId = await SessionUtility.GetUserId();
            List<TaxRate> TaxList = UserUowObj.GetTaxList(UserId);
            ViewBag.Unit = UserUowObj.GetUnitList(UserId).ToList();
            ViewBag.Category = itemUOw.ItemCategoryList(UserId).ToList();
            ViewBag.SubCat = itemUOw.ItemCategorySubList(UserId).ToList();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(TaxList);
        }
        public async Task<IActionResult> MangUnits(int Id, int FirstUnit, int SecondUnit, int Quantity, int AlternateUnitIsApplicables)
        {
            int CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddUCRate(Id, FirstUnit, SecondUnit, Quantity, CreatedBy, AlternateUnitIsApplicables);
            return Json(dict);
        }
        public IActionResult ItemList()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }
        public async Task<IActionResult> ItemGrid(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Item> items = ItemUOw.GetItem(ref TablePaging);
            if (items.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(Item item)
        {
            item.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.AddItem(item);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("ItemList");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("Item");
            }

        }
        public async Task<IActionResult> EditItem(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            Item item = ItemUOw.ItemGetById(id);
            ViewBag.Unit = UserUowObj.GetUnitList(UserId).ToList();
            ViewBag.UCRare = UserUowObj.GetUcRate(item.UCRatesId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.Category = itemUOw.ItemCategoryList(UserId).ToList();
            ViewBag.SubCat = itemUOw.ItemCategorySubList(UserId).ToList();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(Item Itm)
        {
            // Itm.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await ItemUOw.EditItem(Itm);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
            }
            return RedirectToAction("ItemList");

        }
        public IActionResult DeleteItem(int id)
        {
            return Json(itemUOw.DeleteItem(id));
        }

        public IActionResult GetItemTarnsaction(int Id, string name, string From, string To, string Invoice, string Amount, string TrnxTyp, int AseDec)
        {
            return View("ItemTransactions", transactionsUOW.ItemDetails(Id, name, From, To, Invoice, Amount, TrnxTyp, AseDec));
        }


        public IActionResult GetItemDeatil(int id)
        {
            return Json(itemUOw.ItemGetById(id));
        }


        // Transactions

        public async Task<IActionResult> GetInvoiceNo(int TransactionType, string ShopName)
        {
            int UserId = await SessionUtility.GetUserId();
            return Json(transactionsUOW.FindInvoiceNumber(TransactionType, UserId, ShopName));
        }

        public async Task<IActionResult> AddSales()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            ViewBag.states = UserUowObj.GetState();
            ViewBag.City = UserUowObj.GetCityList();
            // ViewBag.ItemList = itemUOw.ItemList(UserId);
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.Shop = UserUowObj.GetShopes(UserId);
            ViewBag.ExtraCharge = UserUowObj.ExtraChargeList(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            if (payemtMode != null)
            {
                List<string> banks = new List<string>();
                if (payemtMode.IsBank == true)
                {
                    banks.Add(PaymentType.Bank.ToString());
                }
                if (payemtMode.IsCash == true)
                {
                    banks.Add(PaymentType.Cash.ToString());
                }
                if (payemtMode.IsWallet)
                {
                    banks.Add(PaymentType.Wallet.ToString());
                }
                if (payemtMode.IsFinance)
                {
                    banks.Add(PaymentType.Finance.ToString());
                }
                ViewBag.Transaction = banks;
            }
            else
            {
                ViewBag.Transaction = null;
            }
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            //  List<string> Desc=
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.Sales);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSales(Transaction transaction)
        {
            if (transaction.TransactionItemsDetails.Count > 0 && transaction.TotalAmount > 0 /* && !transaction.TransactionItemsDetails.Any(x => x.ItemId == 0)*/)
            {
                transaction.CreatedBy = await SessionUtility.GetUserId();
                Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
                if (Convert.ToBoolean(dict["Status"]))
                {
                    ModelState.Clear();
                    ViewBag.Message = Convert.ToString(dict["Message"]);
                    TempData["Message"] = Convert.ToString(dict["Message"]);
                    return RedirectToAction("SalesManage");
                }
                else
                {
                    ViewBag.error = Convert.ToString(dict["Message"]);
                    TempData["error"] = ViewBag.error;
                    return RedirectToAction("AddSales");
                }
            }
            else
            {
                ViewBag.error = "Please enter Fields";
                return RedirectToAction("AddSales");
            }
        }

        public async Task<IActionResult> AddSalesReturan()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            ViewBag.states = UserUowObj.GetState();
            ViewBag.City = UserUowObj.GetCityList();
            // ViewBag.ItemList = itemUOw.ItemList(UserId);
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.Shop = UserUowObj.GetShopes(UserId);
            ViewBag.ExtraCharge = UserUowObj.ExtraChargeList(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            if (payemtMode != null)
            {
                List<string> banks = new List<string>();
                if (payemtMode.IsBank == true)
                {
                    banks.Add(PaymentType.Bank.ToString());
                }
                if (payemtMode.IsCash == true)
                {
                    banks.Add(PaymentType.Cash.ToString());
                }
                if (payemtMode.IsWallet)
                {
                    banks.Add(PaymentType.Wallet.ToString());
                }
                if (payemtMode.IsFinance)
                {
                    banks.Add(PaymentType.Finance.ToString());
                }
                ViewBag.Transaction = banks;
            }
            else
            {
                ViewBag.Transaction = null;
            }
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            //  List<string> Desc=
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.SalesReturn);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSalesReturan(Transaction transaction)
        {
            if (transaction.TransactionItemsDetails.Count > 0 && transaction.TotalAmount > 0 /*&& !transaction.TransactionItemsDetails.Any(x => x.ItemId == 0)*/)
            {
                transaction.CreatedBy = await SessionUtility.GetUserId();
                Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
                if (Convert.ToBoolean(dict["Status"]))
                {
                    ModelState.Clear();
                    ViewBag.Message = Convert.ToString(dict["Message"]);
                    TempData["Message"] = Convert.ToString(dict["Message"]);
                    return RedirectToAction("SalesManage");
                }
                else
                {
                    ViewBag.error = Convert.ToString(dict["Message"]);
                    TempData["error"] = ViewBag.error;
                    return RedirectToAction("AddSalesReturan");
                }
            }
            else
            {
                ViewBag.error = "Please enter Fields";
                return RedirectToAction("AddSalesReturan");
            }
        }

        // Edit Sales
        public async Task<IActionResult> Editsalese(int id)

        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.TrnxItem = transactionsUOW.GetTransactionById(id);
            ViewBag.TrnxData = transactionsUOW.GetTrnxData(id);
            List<TransactionItemDetail> transactions = transactionsUOW.GetItemTnxFrmTrnxId(id);
            return View(transactions);
        }

        public async Task<IActionResult> SalesMangae(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Transaction> ModelList = transactionsUOW.GetSales(ref TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }

        public async Task<IActionResult> SalesManage()
        {
            //int UserId = await SessionUtility.GetUserId();
            //List<Transaction> transactions = transactionsUOW.SalesTRNX(UserId);
            //List<Transaction> SalesReturn = transactionsUOW.SalesReturnTRNX(UserId);

            //var Paidsale = transactions.Select(a => a.RecievedAmt).ToList().Sum();
            //var DueBalsale = transactions.Select(a => a.RemainingAmt).ToList().Sum();
            //var totalsale = transactions.Select(a => a.TotalAmount).ToList().Sum();

            //var PaidsaleRet = SalesReturn.Select(a => a.RecievedAmt).ToList().Sum();
            //var DueBalsaleRet = SalesReturn.Select(a => a.RemainingAmt).ToList().Sum();
            //var totalsaleRet = SalesReturn.Select(a => a.TotalAmount).ToList().Sum();

            //ViewBag.Paid = Paidsale - PaidsaleRet;
            //ViewBag.DueBal = DueBalsale - DueBalsaleRet;
            //ViewBag.total = totalsale - totalsaleRet;
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        public IActionResult DeletTaransaction(int id)
        {
            return Json(transactionsUOW.DeleteTrnx(id));
        }

        // Payment In
        public async Task<IActionResult> PaymentIn()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            List<string> banks = new List<string>();
            if (payemtMode.IsBank == true)
            {
                banks.Add(PaymentType.Bank.ToString());
            }
            if (payemtMode.IsCash == true)
            {
                banks.Add(PaymentType.Cash.ToString());
            }
            if (payemtMode.IsWallet)
            {
                banks.Add(PaymentType.Wallet.ToString());
            }
            if (payemtMode.IsFinance)
            {
                banks.Add(PaymentType.Finance.ToString());
            }
            ViewBag.Transaction = banks;
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.PaymentIn);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        public async Task<IActionResult> PaymentInGrid(int Id)
        {
            List<Transaction> ModelList = transactionsUOW.GetSalesDueAmt(Id);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            return View(ModelList);
        }
        [HttpPost]
        public async Task<IActionResult> PaymentIn(Transaction transaction)
        {
            transaction.CreatedBy = await SessionUtility.GetUserId();
            transaction.TotalAmount = transaction.RecievedAmt;
            Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("PaymentIn");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("PaymentIn");
            }

        }
        // Bank
        public async Task<IActionResult> Bank()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            List<BankAccount> Bank = UserUowObj.GetBankList(UserId);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(Bank);
        }
        [HttpPost]
        public async Task<IActionResult> AddBank(BankAccount bank)
        {
            bank.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddBanks(bank);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("Bank");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("Bank");
            }

        }

        [HttpPost]
        public async Task<IActionResult> BankSatelment(BankTrnxInfo bankTrnx)
        {
            bankTrnx.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await transactionsUOW.BankSatelment(bankTrnx);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("Bank");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("Bank");
            }

        }

        public async Task<IActionResult> BankTransactions(string BankName, string name)
        {
            int UserId = await SessionUtility.GetUserId();
            List<BankTrnxInfo> ModelList = transactionsUOW.BnakTransactonts(BankName, name, UserId);

            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(ModelList);
        }
        public IActionResult GetBankDetail(int id)

        {
            return Json(UserUowObj.BankDetail(id));
        }

        public IActionResult deleteBank(int id)
        {
            return Json(UserUowObj.DeleteBank(id));
        }
        // Cash   
        public async Task<IActionResult> CashAccount()
        {
            int CreatedBy = await SessionUtility.GetUserId();
            List<CashTransaction> cashes = transactionsUOW.GetCashTranx(CreatedBy);
            return View(cashes);
        }
        [HttpPost]
        public async Task<IActionResult> CashSatelment(CashTransaction cash)
        {
            cash.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await transactionsUOW.CashSatelment(cash);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("CashAccount");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("CashAccount");
            }
        }

        public async Task<IActionResult> CashAccountGrid(string name)
        {
            int CurrentUserId = await SessionUtility.GetUserId();
            List<CashTransaction> ModelList = transactionsUOW.CashTransactonts(name, CurrentUserId);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }

            return View(ModelList);
        }

        public async Task<IActionResult> WalletAccount()
        {
            int CreatedBy = await SessionUtility.GetUserId();
            List<WalletInfo> wallets = transactionsUOW.GetWalletInfoTrn(CreatedBy);
            return View(wallets);
        }
        public async Task<IActionResult> WalletAccountGrid(string name)
        {
            int CurrentUserId = await SessionUtility.GetUserId();
            List<WalletInfo> ModelList = transactionsUOW.WalletTransactonts(name, CurrentUserId);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }

            return View(ModelList);
        }

        public async Task<IActionResult> Cheques()
        {
            return View();
        }
        public async Task<IActionResult> ChequeAccountGrid(string name)
        {
            int CurrentUserId = await SessionUtility.GetUserId();
            List<BankTrnxInfo> ModelList = transactionsUOW.ChequeTransactonts(name, CurrentUserId);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }

            return View(ModelList);
        }
        public async Task<IActionResult> ItemData(int ItemIds)
        {

            Dictionary<string, object> dict = await ItemUOw.GetItemById(ItemIds);

            return Json(dict);
        }
        public IActionResult TaxRatesInfo(int TaxRats)
        {
            TaxRate tax = UserUowObj.GetTaxById(TaxRats);
            return Json(tax);
        }

        // purchase
        public async Task<IActionResult> AddPurchase()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            ViewBag.states = UserUowObj.GetState();
            ViewBag.City = UserUowObj.GetCityList();
            // ViewBag.ItemList = itemUOw.ItemList(UserId);
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.Shop = UserUowObj.GetShopes(UserId);
            ViewBag.ExtraCharge = UserUowObj.ExtraChargeList(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            if (payemtMode != null)
            {
                List<string> banks = new List<string>();
                if (payemtMode.IsBank == true)
                {
                    banks.Add(PaymentType.Bank.ToString());
                }
                if (payemtMode.IsCash == true)
                {
                    banks.Add(PaymentType.Cash.ToString());
                }
                if (payemtMode.IsWallet)
                {
                    banks.Add(PaymentType.Wallet.ToString());
                }
                if (payemtMode.IsFinance)
                {
                    banks.Add(PaymentType.Finance.ToString());
                }
                ViewBag.Transaction = banks;
            }
            else
            {
                ViewBag.Transaction = null;
            }
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            //  List<string> Desc=
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.Purchase);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchase(Transaction transaction)
        {

            if (transaction.TransactionItemsDetails.Count > 0 && transaction.TotalAmount > 0 /*&& !transaction.TransactionItemsDetails.Any(x => x.ItemId == 0)*/)
            {
                transaction.CreatedBy = await SessionUtility.GetUserId();
                Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
                if (Convert.ToBoolean(dict["Status"]))
                {
                    ModelState.Clear();
                    ViewBag.Message = Convert.ToString(dict["Message"]);
                    TempData["Message"] = Convert.ToString(dict["Message"]);
                    return RedirectToAction("purchaseManage");

                }
                else
                {
                    ViewBag.error = Convert.ToString(dict["Message"]);
                    TempData["error"] = ViewBag.error;
                    return RedirectToAction("AddPurchase");

                }
            }
            else
            {
                ViewBag.error = "Please Fill Fields";
                return RedirectToAction("AddPurchase");

            }
        }

        public async Task<IActionResult> AddPurchaseReturn()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            ViewBag.states = UserUowObj.GetState();
            ViewBag.City = UserUowObj.GetCityList();
            //  ViewBag.ItemList = itemUOw.ItemList(UserId);
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.Shop = UserUowObj.GetShopes(UserId);
            ViewBag.ExtraCharge = UserUowObj.ExtraChargeList(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            if (payemtMode != null)
            {
                List<string> banks = new List<string>();
                if (payemtMode.IsBank == true)
                {
                    banks.Add(PaymentType.Bank.ToString());
                }
                if (payemtMode.IsCash == true)
                {
                    banks.Add(PaymentType.Cash.ToString());
                }
                if (payemtMode.IsWallet)
                {
                    banks.Add(PaymentType.Wallet.ToString());
                }
                if (payemtMode.IsFinance)
                {
                    banks.Add(PaymentType.Finance.ToString());
                }
                ViewBag.Transaction = banks;
            }
            else
            {
                ViewBag.Transaction = null;
            }
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            //  List<string> Desc=
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.PurchaseReturn);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchaseReturn(Transaction transaction)
        {
            if (transaction.TransactionItemsDetails.Count > 0 && transaction.TotalAmount > 0 /*&& !transaction.TransactionItemsDetails.Any(x => x.ItemId == 0)*/)
            {
                transaction.CreatedBy = await SessionUtility.GetUserId();
                Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
                if (Convert.ToBoolean(dict["Status"]))
                {
                    ModelState.Clear();
                    ViewBag.Message = Convert.ToString(dict["Message"]);
                    TempData["Message"] = Convert.ToString(dict["Message"]);
                    return RedirectToAction("purchaseManage");

                }
                else
                {
                    ViewBag.error = Convert.ToString(dict["Message"]);
                    TempData["error"] = ViewBag.error;
                    return RedirectToAction("AddPurchaseReturn");

                }
            }
            else
            {
                ViewBag.error = "Please Fill Fields";
                return RedirectToAction("AddPurchaseReturn");

            }
        }

        public async Task<IActionResult> PrchaseMangae(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Transaction> ModelList = transactionsUOW.GetPurchase(ref TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }

        public async Task<IActionResult> purchaseManage()
        {
            //int UserId = await SessionUtility.GetUserId();
            //List<Transaction> transactions = transactionsUOW.PurchaseTRNX(UserId);
            //List<Transaction> purchasereturn = transactionsUOW.PurchaseReturn(UserId);

            //var Paid = transactions.Select(a => a.RecievedAmt).ToList().Sum();
            //var DueBal = transactions.Select(a => a.RemainingAmt).ToList().Sum();
            //var total = transactions.Select(a => a.TotalAmount).ToList().Sum();

            //var PaidRet = purchasereturn.Select(a => a.RecievedAmt).ToList().Sum();
            //var DueBalRet = purchasereturn.Select(a => a.RemainingAmt).ToList().Sum();
            //var totalRet = purchasereturn.Select(a => a.TotalAmount).ToList().Sum();

            //ViewBag.Paid = total - PaidRet;
            //ViewBag.DueBal = DueBal - DueBalRet;
            //ViewBag.total = total - totalRet;
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }

            return View();
        }

        // Payment Out
        public async Task<IActionResult> PaymentOut()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            List<string> banks = new List<string>();
            if (payemtMode.IsBank == true)
            {
                banks.Add(PaymentType.Bank.ToString());
            }
            if (payemtMode.IsCash == true)
            {
                banks.Add(PaymentType.Cash.ToString());
            }
            if (payemtMode.IsWallet)
            {
                banks.Add(PaymentType.Wallet.ToString());
            }
            if (payemtMode.IsFinance)
            {
                banks.Add(PaymentType.Finance.ToString());
            }
            ViewBag.Transaction = banks;
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.PaymentOut);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }
        public async Task<IActionResult> PaymentOutGrid(int Id)
        {
            List<Transaction> ModelList = transactionsUOW.GetPurchaseDueAmt(Id);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            return View(ModelList);
        }
        [HttpPost]
        public async Task<IActionResult> PaymentOut(Transaction transaction)
        {
            transaction.CreatedBy = await SessionUtility.GetUserId();
            transaction.TotalAmount = transaction.RecievedAmt;
            Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("PaymentOut");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("PaymentOut");
            }
        }
        // Expenses
        public async Task<IActionResult> Expenses()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            ViewBag.states = UserUowObj.GetState();
            ViewBag.City = UserUowObj.GetCityList();
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.Shop = UserUowObj.GetShopes(UserId);
            ViewBag.ExtraCharge = UserUowObj.ExtraChargeList(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            if (payemtMode != null)
            {
                List<string> banks = new List<string>();
                if (payemtMode.IsBank == true)
                {
                    banks.Add(PaymentType.Bank.ToString());
                }
                if (payemtMode.IsCash == true)
                {
                    banks.Add(PaymentType.Cash.ToString());
                }
                if (payemtMode.IsWallet)
                {
                    banks.Add(PaymentType.Wallet.ToString());
                }
                if (payemtMode.IsFinance)
                {
                    banks.Add(PaymentType.Finance.ToString());
                }
                ViewBag.Transaction = banks;
            }
            else
            {
                ViewBag.Transaction = null;
            }
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            //  List<string> Desc=
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.Expenses);



            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        public async Task<IActionResult> GetExpensesItem(int id)
        {
            ViewBag.indx = id;
            int UserId = await SessionUtility.GetUserId();
            ViewBag.ItemList = itemUOw.ExpenseItemList(UserId).ToList();
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Expenses(Transaction expense)
        {
            expense.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await transactionsUOW.Transaction(expense);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("Expenses");

            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("Expenses");
            }

        }

        public async Task<IActionResult> ExpensesItems()
        {
            int UserId = await SessionUtility.GetUserId();
            List<Item> categories = itemUOw.ExpenseItemList(UserId).ToList();
            if (categories.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.TaxRate = UserUowObj.GetTaxList(UserId);
            ViewBag.Category = itemUOw.ExpensesCategoryList(UserId).ToList();
            ViewBag.SubCat = ItemUOw.ExpenseCategorySubList(UserId).ToList(); ;

            return View(categories);

        }
        [HttpPost]
        public async Task<IActionResult> AddExpensesItem(Item item)
        {
            item.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.AddItem(item);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("ExpensesItems");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
                return RedirectToAction("ExpensesItems");

            }

        }

        [HttpPost]
        public async Task<IActionResult> EditExpenseItem(Item Itm)
        {
            Dictionary<string, object> dict = await ItemUOw.EditItem(Itm);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
            }

            return RedirectToAction("ExpensesItems");

        }
        public async Task<IActionResult> ExpenseaCategory()
        {
            int UserId = await SessionUtility.GetUserId();
            List<ItemCategory> expense = itemUOw.ExpensesCategoryList(UserId);
            ViewBag.LegderGrp = UserUowObj.ledgerGroups(UserId);
            return View(expense);

        }
        public async Task<IActionResult> GetExpenseCatItem(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            ItemCategory item = ItemUOw.GetItemCategory(id);
            return Json(item);
        }

        [HttpPost]
        public async Task<IActionResult> ExpenseaCategory(ItemCategory expense)
        {
            expense.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.AddItemCategoty(expense);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("ExpenseaCategory");

            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("ExpenseaCategory");
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditExpenseCat(ItemCategory Itm)
        {
            Dictionary<string, object> dict = await ItemUOw.EditItemCategoty(Itm);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
            }

            return RedirectToAction("ExpenseaCategory");

        }

        public IActionResult deleteExpeCateGory(int id)
        {
            return Json(itemUOw.DeleteItemCategoty(id));
        }

        public async Task<IActionResult> ExpenseSubCategory()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.IncomeCatecogy = ItemUOw.ExpensesCategoryList(UserId).ToList();
            List<ItemSubCategory> incomeSubs = ItemUOw.ExpenseCategorySubList(UserId).ToList();
            return View(incomeSubs);
        }
        [HttpPost]
        public async Task<IActionResult> ExpenseSubCategory(ItemSubCategory incomeSub)
        {
            incomeSub.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await ItemUOw.AddItemSubCategoty(incomeSub);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("ExpenseSubCategory");

            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
                return RedirectToAction("ExpenseSubCategory");
            }
        }
        public async Task<IActionResult> GetExpenseSubCatItem(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            ItemSubCategory item = ItemUOw.GetSubCategory(id);
            return Json(item);
        }
        [HttpPost]
        public async Task<IActionResult> EditExpensesSubCatItem(ItemSubCategory Itm)
        {

            Dictionary<string, object> dict = await ItemUOw.EditItemSubCategoty(Itm);

            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.msg = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["Message"] = ViewBag.error;
            }
            return RedirectToAction("ExpenseSubCategory");

        }

        public IActionResult deleteExpensesSubCateGory(int id)
        {
            return Json(ItemUOw.DeleteItemSubCategoty(id));
        }



        [HttpPost]
        public async Task<IActionResult> Expens(Transaction expense)
        {
            expense.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await transactionsUOW.Transaction(expense);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("ExpenseaCategory");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("ExpenseaCategory");
            }
        }
        public async Task<IActionResult> ExpensesList()
        {
            int UserId = await SessionUtility.GetUserId();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        public async Task<IActionResult> ExpensesGrid(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Transaction> ModelList = transactionsUOW.GetExpenses(ref TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }


        public async Task<IActionResult> PaymentOutExpenses()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.PartyList = UserUowObj.GetPartys(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            List<string> banks = new List<string>();
            if (payemtMode.IsBank == true)
            {
                banks.Add(PaymentType.Bank.ToString());
            }
            if (payemtMode.IsCash == true)
            {
                banks.Add(PaymentType.Cash.ToString());
            }
            if (payemtMode.IsWallet)
            {
                banks.Add(PaymentType.Wallet.ToString());
            }
            if (payemtMode.IsFinance)
            {
                banks.Add(PaymentType.Finance.ToString());
            }
            ViewBag.Transaction = banks;
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.PaymentOutExpenses);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }

        public IActionResult PaymentOutExpensesGrid(int Id)
        {
            List<Transaction> ModelList = transactionsUOW.GetExpensesDueAmt(Id);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            return View(ModelList);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentOutExpenses(Transaction transaction)
        {
            transaction.CreatedBy = await SessionUtility.GetUserId();
            transaction.TotalAmount = transaction.RecievedAmt;
            Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("PaymentOutExpenses");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("PaymentOutExpenses");
            }
        }

        public IActionResult ReportOFAllTransactions()
        {
            return View();
        }
        public async Task<IActionResult> ReportOFAllTransactionsGrid(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Transaction> ModelList = transactionsUOW.AllTransactonts(TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }

        public IActionResult ProfitAndLoss()
        {
            return View();
        }
        public async Task<IActionResult> ProfitLoss(string FromDate, string ToDate)
        {
            int UserId = await SessionUtility.GetUserId();
            if (FromDate == null)
            {
                DateTime today = DateUtility.GetNowTime();
                DateTime sixMonthsBack = today.AddMonths(-6);
                today.ToShortDateString();
                sixMonthsBack.ToShortDateString();
                FromDate = sixMonthsBack.ToString("yyyy-MM-dd");
                ToDate = today.ToString("yyyy-MM-dd");

            }
            Sp_ProfitLoss ModelList = UserUowObj.GetProfitLost(UserId, FromDate, ToDate);
            if (ModelList == null)
            {
                ViewData["Message"] = ("No Records found");
            }
            return View(ModelList);
        }

        public IActionResult BalanceSheet()
        {
            return View();
        }

        public async Task<IActionResult> BalanceSheetGrid(string FromDate, string ToDate)
        {
            int UserId = await SessionUtility.GetUserId();
            SP_BalanceSheet ModelList = UserUowObj.GetBalanceSheet(UserId, FromDate, ToDate);
            return View(ModelList);

        }

        public IActionResult DayBook()
        {

            return View();
        }
        public async Task<IActionResult> DayBookGrid(string FromDate)
        {
            int UserID = await SessionUtility.GetUserId();
            List<Transaction> ModelList = transactionsUOW.AllTransactontsOfDay(UserID, FromDate);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            //  ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);

        }

        public async Task<IActionResult> GetTxnItem(int id)
        {
            ViewBag.indx = id;
            int UserId = await SessionUtility.GetUserId();
            ViewBag.ItemList = itemUOw.ItemList(UserId).ToList();
            ViewBag.Unit = UserUowObj.GetUnitList(UserId);
            ViewBag.Tax = UserUowObj.GetTaxList(UserId);
            ViewBag.UserInfo = UserUowObj.GetById(UserId);
            return View();
        }
        public async Task<IActionResult> InvoiceNo()
        {
            // int UserId = await SessionUtility.GetUserId();
            // Invoice_no invoice_No = UserUowObj.GetInvoiceDetail(UserId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InvoiceNo(Invoice_no invoice)
        {
            invoice.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.InvoiceNumber(invoice);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("InvoiceNo");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("InvoiceNo");
            }
            //  return View();
        }
        public async Task<IActionResult> GetInvoiceDetail(int TrnxType)
        {
            int CreatedBy = await SessionUtility.GetUserId();
            return Json(UserUowObj.GetInvoice_No(CreatedBy, TrnxType));
        }
        public IActionResult DeleteShop(int Id)
        {
            return Json(UserUowObj.DeleteShope(Id));
        }
        public async Task<IActionResult> ShopGrid()
        {
            int UserId = await SessionUtility.GetUserId();
            List<Shopes> shopes = UserUowObj.GetShopes(UserId);
            return View(shopes);
        }
        [HttpPost]
        public async Task<IActionResult> AddShop(string ShopName, string LableForInvoice)
        {
            Shopes shopes = new Shopes();
            shopes.ShopName = ShopName;
            shopes.LableForInvoice = LableForInvoice;
            shopes.CreatedBy = await SessionUtility.GetUserId();
            return Json(UserUowObj.AddShopes(shopes));
        }
        public async Task<IActionResult> PaymentMode()
        {
            int UserId = await SessionUtility.GetUserId();
            PayemtModeOption payemt = UserUowObj.GetPaymentMode(UserId);
            return View(payemt);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentMode(PayemtModeOption payment)
        {
            payment.createdBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.PaymentMode(payment);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("PaymentMode");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("PaymentMode");
            }
            //  return View();
        }


        public async Task<IActionResult> TrialBalance()
        {
            return View();
        }
        public async Task<IActionResult> TrialBalanceGrid(string FromDate, string ToDate)
        {
            int UserId = await SessionUtility.GetUserId();
            if (FromDate == null)
            {
                DateTime today = DateUtility.GetNowTime();
                DateTime sixMonthsBack = today.AddMonths(-6);
                today.ToShortDateString();
                sixMonthsBack.ToShortDateString();
                FromDate = sixMonthsBack.ToString("yyyy-MM-dd");
                ToDate = today.ToString("yyyy-MM-dd");
            }
            Sp_TrialBalance ModelList = UserUowObj.GetSp_TrialBalance(UserId, FromDate, ToDate);
            if (ModelList == null)
            {
                ViewData["Message"] = ("No Records found");
            }
            return View(ModelList);
        }

        public async Task<IActionResult> Descration()
        {
            int UserId = await SessionUtility.GetUserId();
            List<Description> descriptions = UserUowObj.GetDescription(UserId);
            return View(descriptions);
        }
        public async Task<IActionResult> DescraptionSearch(string search, int TrnxType)
        {
            int UserId = await SessionUtility.GetUserId();
            List<Description> descriptions = UserUowObj.GetDescription(UserId).Where(a => a.Descriptions.ToLower().Contains(search.ToLower()) && (a.TransactionType == TrnxType || a.TransactionType == 0)).ToList();
            return Json(descriptions);
        }
        [HttpPost]
        public async Task<IActionResult> Descration(Description description)
        {
            description.CreateBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddDescription(description);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("Descration");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("Descration");
            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveDescration(string search, int TrnxType)
        {
            Description description = new Description();
            description.CreateBy = await SessionUtility.GetUserId();
            description.Descriptions = search;
            description.TransactionType = TrnxType;
            return Json(UserUowObj.AddDescription(description));
        }
        public IActionResult DeleteDescration(int Id)
        {
            return Json(UserUowObj.DeleteDescription(Id));
        }

        public async Task<IActionResult> ExtraChargeCreate()
        {
            int UserId = await SessionUtility.GetUserId();
            List<TaxRate> TaxList = UserUowObj.GetTaxList(UserId);
            ViewBag.Category = TaxList;
            ViewBag.LedgerGrp = UserUowObj.ledgerGroups(UserId);

            return View();
        }

        public async Task<IActionResult> ExtraChargeGrid(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<ExtraCharge> ModelList = UserUowObj.ExtraChargeList(ref TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            //  ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }

        public async Task<IActionResult> GetExtraChargeInfo(int search)
        {
            int UserId = await SessionUtility.GetUserId();
            ExtraCharge extraCharge = UserUowObj.ExtraChargeList(UserId).Where(a => a.Id == search).FirstOrDefault();
            return Json(extraCharge);
        }
        [HttpPost]
        public async Task<IActionResult> ExtraChargeCreate(ExtraCharge extraCharge)
        {
            extraCharge.CreateBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.CreateExtraCharge(extraCharge);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("ExtraChargeCreate");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("ExtraChargeCreate");
            }

        }

        public IActionResult DeleteExtraCharge(int id)
        {
            return Json(UserUowObj.DeleteExtraCharge(id));
        }

        public IActionResult GSTReport()
        {
            return View();

        }
        public IActionResult GSTReportGrid(int Id, string name, string From, string To, string Invoice, string Amount, int AseDec)
        {
            return View(transactionsUOW.GetIgstTransaction(Id, name, From, To, Invoice, Amount, AseDec));
        }
        public async Task<IActionResult> GSTReportDetail(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.TrnxItem = transactionsUOW.GetTransactionById(id);
            ViewBag.TrnxData = transactionsUOW.GetTrnxData(id);
            List<TransactionItemDetail> transactions = transactionsUOW.GetItemTnxFrmTrnxId(id);
            return View(transactions);
        }
        public IActionResult CGSTReport()
        {
            return View();
        }
        public IActionResult CGSTReportGrid(int Id, string name, string From, string To, string Invoice, string Amount, int AseDec)
        {
            return View(transactionsUOW.GetSGstTransaction
                (Id, name, From, To, Invoice, Amount, AseDec));
        }
        public async Task<IActionResult> CGSTReportDetail(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.TrnxItem = transactionsUOW.GetTransactionById(id);
            ViewBag.TrnxData = transactionsUOW.GetTrnxData(id);
            List<TransactionItemDetail> transactions = transactionsUOW.GetItemTnxFrmTrnxId(id);
            return View(transactions);
        }
        public IActionResult SGSTReport()
        {
            return View();
        }
        public IActionResult SGTReportGrid(int Id, string name, string From, string To, string Invoice, string Amount, int AseDec)
        {
            return View(transactionsUOW.GetSGstTransaction(Id, name, From, To, Invoice, Amount, AseDec));
        }
        public async Task<IActionResult> SGTReportDetail(int id)

        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.TrnxItem = transactionsUOW.GetTransactionById(id);
            ViewBag.TrnxData = transactionsUOW.GetTrnxData(id);
            List<TransactionItemDetail> transactions = transactionsUOW.GetItemTnxFrmTrnxId(id);
            return View(transactions);
        }

        public IActionResult RoundOffReport()
        {
            return View();
        }

        public IActionResult RoundOffReportGrid(int Id, string name, string From, string To, string Invoice, string Amount, int AseDec)
        {
            return View(transactionsUOW.GetRoundOffTransaction(Id, name, From, To, Invoice, Amount, AseDec));
        }


        public IActionResult ExtraCharges()
        {
            return View();
        }
        
        public IActionResult ExtraChargesGrid(int Id, string name, string From, string To, string Invoice, string Amount, int AseDec)
        {
            return View(transactionsUOW.GetExtraChargeTransction(Id, name, From, To, Invoice, Amount, AseDec));
        }
        public async Task<IActionResult> ExtraChargesDetail(int id)
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.TrnxItem = transactionsUOW.GetTransactionById(id);
            ViewBag.TrnxData = transactionsUOW.GetTrnxData(id);
            List<ExtraChargeData> extraCharges = transactionsUOW.ExtraChargeListGetByTrnxId(id);
            return View(extraCharges);
        }

        public IActionResult ExtraCharegesReport()
        {
            return View();
        }
        public async Task<IActionResult> ExtraChargeList(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<ExtraCharge> ModelList = UserUowObj.ExtraChargeList(ref TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            //  ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }
        public IActionResult ExtraChargeTarnsaction(int Id, string name, string From, string To, string Invoice, string Amount, int AseDec)
        {
            return View(transactionsUOW.ExtraChargeTransactonts(Id, name, From, To, Invoice, Amount, AseDec));
        }

        public IActionResult ExtraChargeDetail(int id)
        {
            return Json(UserUowObj.GetExtraxchargeById(id));
        }
        
        [HttpPost]
        public IActionResult DeleteLedger(int id)
        {
            return Json(ledgerUOW.DeleteLedger(id));
        }
        public async Task<IActionResult> LeddgerReport()
        {
            int UserId = await SessionUtility.GetUserId();
            List<State> states = UserUowObj.GetState();
            List<LedgerSubGroup> ledgersub = UserUowObj.ledgerSubGroups(UserId);
            List<LedgerGroup> ledgers = UserUowObj.ledgerGroups(UserId);
            ViewBag.states = states;
            ViewBag.ledgers = ledgers;
            ViewBag.ledgersub = ledgersub;
            return View();
        }
        public async Task<IActionResult> AddLedger()
        {
            int UserId = await SessionUtility.GetUserId();
            List<State> states = UserUowObj.GetState();
            List<LedgerSubGroup> ledgersub = UserUowObj.ledgerSubGroups(UserId);
            List<LedgerGroup> ledgers = UserUowObj.ledgerGroups(UserId);
            ViewBag.states = states;
            ViewBag.ledgers = ledgers;
            ViewBag.ledgersub = ledgersub;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddLedger(Ledger ledger)
        {
            ledger.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await ledgerUOW.AddLedger(ledger);
            if (Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("LeddgerReport");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
                TempData["error"] = ViewBag.error;
                return RedirectToAction("LeddgerReport");
            }
        }
        public async Task<IActionResult> LedgerList(DataPagingModel TablePaging)
        {
            TablePaging.CurrentUserId = await SessionUtility.GetUserId();
            List<Ledger> ModelList = ledgerUOW.LedgersList(ref TablePaging);
            if (ModelList.Count <= 0)
            {
                ViewData["Message"] = ("No Records found");
            }
            //  ViewBag.DataPagingModel = TablePaging;
            return View(ModelList);
        }
        public async Task<IActionResult> EditLedger(int id)
        {
            Ledger ledger = ledgerUOW.GetLedre(id);
            int UserId = await SessionUtility.GetUserId();
            List<State> states = UserUowObj.GetState();
            List<LedgerSubGroup> ledgersub = UserUowObj.ledgerSubGroups(UserId);
            List<LedgerGroup> ledgers = UserUowObj.ledgerGroups(UserId);
            ViewBag.states = states;
            ViewBag.ledgers = ledgers;
            ViewBag.ledgersub = ledgersub;
            return View(ledger);
        }
        [HttpPost]
        public async Task<IActionResult> EditLedger(Ledger ledger)
        {
            Dictionary<string, object> dict = ledgerUOW.EditLedger(ledger);
            if(Convert.ToBoolean(dict["Status"]))
            {
                ModelState.Clear();
                ViewBag.Message = Convert.ToString(dict["Message"]);
                TempData["Message"] = Convert.ToString(dict["Message"]);
                return RedirectToAction("LeddgerReport");
            }
            else
            {
                ViewBag.error = Convert.ToString(dict["Message"]);
             
                TempData["error"] = ViewBag.error;
                return RedirectToAction("LeddgerReport");
            }
        }
        public IActionResult LedgerDetails(int id)
        {
            return Json(ledgerUOW.GetLedre(id));
        }

        public async Task<IActionResult> PartyItemPartial()
        {
            int UserId = await SessionUtility.GetUserId();
            //  List<MyAppUser> CleienList = UserUowObj.GetClient();
            List<State> states = UserUowObj.GetState();
            List<LedgerSubGroup> ledgers = UserUowObj.ledgerSubGroups(UserId);
            ViewBag.states = states;
            ViewBag.ledgers = ledgers;
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View(ledgers);
        }
        public async Task<IActionResult> AddPatryByJson(MyAppUser User)
        {
            User.ClientId = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddParty(User);
            if (Convert.ToBoolean(dict["Status"]))
            {
                MyAppUser users = UserUowObj.GetUserByMobile(User.PhoneNumber);
                return Json(users);
            }
            else
            {
                return Json(false);
            }
        }
        public async Task<IActionResult> ItemPartial()
        {
            int UserId = await SessionUtility.GetUserId();
            List<TaxRate> TaxList = UserUowObj.GetTaxList(UserId);
            ViewBag.Unit = UserUowObj.GetUnitList(UserId).ToList();
            ViewBag.Category = itemUOw.ItemCategoryList(UserId).ToList();
            ViewBag.SubCat = itemUOw.ItemCategorySubList(UserId).ToList();
            return View(TaxList);
        }
        [HttpPost]
        public async Task<IActionResult> AddItemByJson(Item item)
        {
            item.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await itemUOw.AddItem(item);
            if (Convert.ToBoolean(dict["Status"]))
            {
              
                return Json(dict["Item"]);
            }
            else
            { 
                return Json(false);
            }
        }
        public async Task<IActionResult> LedgerGroupPartial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLedgerGroupByJson(LedgerGroup ledger)
        {
            ledger.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddLedgerGroup(ledger);
            if (Convert.ToBoolean(dict["Status"]))
            {
               return Json(dict["Data"]);
            }
            else
            {
                return Json(false);
            }
        }
        public async Task<IActionResult> LedgerSubGroupPartial()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.ledgerGroups = UserUowObj.ledgerGroups(UserId);
            List<LedgerSubGroup> ledgerSubGroups = UserUowObj.ledgerSubGroups(UserId);
            return View(ledgerSubGroups);
        }

        [HttpPost]
        public async Task<IActionResult> AddLedgerSubGroupByJson(LedgerSubGroup ledger)
        {
            ledger.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddLedgerSubGroup(ledger);
            if (Convert.ToBoolean(dict["Status"]))
            {

                return Json(dict["Deta"]);
            }
            else
            {
                return Json(false);
            }
        }

        public async Task<IActionResult> AddBank()
        {
            int UserId = await SessionUtility.GetUserId();
            List<State> states = UserUowObj.GetState();
            List<LedgerSubGroup> ledgersub = UserUowObj.ledgerSubGroups(UserId);
            List<LedgerGroup> ledgers = UserUowObj.ledgerGroups(UserId);
            ViewBag.states = states;
            ViewBag.ledgers = ledgers;
            ViewBag.ledgersub = ledgersub;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBankByJson(BankAccount bank)
        {
            bank.CreatedBy = await SessionUtility.GetUserId();
            Dictionary<string, object> dict = await UserUowObj.AddBanks(bank);
            if (Convert.ToBoolean(dict["Status"]))
            {
                return Json(dict);
            }
            else
            {
                return Json(dict);
            }
        }

        public async Task<IActionResult> Creditors()
        {
            return View();
        }
        public async Task<IActionResult> Debetors()
        {
            return View();
        }

        public async Task<IActionResult> LedgerTransaction()
        {
            int UserId = await SessionUtility.GetUserId();
            ViewBag.LedgerList = ledgerUOW.AllLedgerList(UserId);
            PayemtModeOption payemtMode = UserUowObj.GetPaymentMode(UserId);
            List<string> banks = new List<string>();
            if (payemtMode.IsBank == true)
            {
                banks.Add(PaymentType.Bank.ToString());
            }
            if (payemtMode.IsCash == true)
            {
                banks.Add(PaymentType.Cash.ToString());
            }
            if (payemtMode.IsWallet)
            {
                banks.Add(PaymentType.Wallet.ToString());
            }
            if (payemtMode.IsFinance)
            {
                banks.Add(PaymentType.Finance.ToString());
            }
            ViewBag.Transaction = banks;
            ViewBag.Bank = UserUowObj.GetBankList(UserId).ToList();
            ViewBag.InvoiceNo = UserUowObj.GetInvoice_No(UserId, (int)TrsnsacctionsType.PaymentInIncome);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
            }
            return View();
        }
        public IActionResult LedgerDetals(int id)
        {
            Ledger ledger = ledgerUOW.GetLedre(id);
            return Json(ledger);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentForParty (Transaction transaction)
        {
            if (transaction.PartyId > 0 && transaction.RecievedAmt >0  /* && !transaction.TransactionItemsDetails.Any(x => x.ItemId == 0)*/)
            {
                transaction.TotalAmount = transaction.RecievedAmt;
                transaction.CreatedBy = await SessionUtility.GetUserId();
                Dictionary<string, object> dict = await transactionsUOW.Transaction(transaction);
                if (Convert.ToBoolean(dict))
                {
                    return Json(dict);
                }
                else
                {
                    return Json(dict);
                }
            }
            else
            {
                string Message = "Please enter values";
                return Json(Message);
            }
        }

    }
}
