﻿using ServiceLayerPattern.Presentation;
using ServiceLayerPattern.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceLayerPattern.WebUI.WebForm
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayTicketReservations();
            }
        }

        private void DisplayTicketReservations()
        {
            lblBasketContents.Text = Basket.GetBasket().Reservation.Description;
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            TicketServiceFacade ticketService = new TicketServiceFacade(new TicketServiceClientProxy());
            TicketPresentation ticket = ticketService.PurchaseReservedTicket(Basket.GetBasket().Reservation.EventId, Basket.GetBasket().Reservation.ReservationId.ToString());

            DisplayTicketReservations();

            if (ticket.WasAbleToPurchaseTicket)
                lblThankYou.Text = "<h2>Thank you for your order.</h2>" + ticket.Description;
            else
                lblThankYou.Text = "<h2>Sorry there was a problem with your order.</h2>" + ticket.Description;
        }
    }
}