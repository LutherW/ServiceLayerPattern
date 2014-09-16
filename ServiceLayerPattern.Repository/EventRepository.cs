using ServiceLayerPattern.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ServiceLayerPattern.Repository
{
    public class EventRepository : IEventRepository
    {
        private static readonly string connectionString = "data source=127.0.0.1;initial catalog=EventTickets;integrated security=false;persist security info=True;User ID=sa;Password=wangtinglu";
             
        public Event FindBy(Guid id)
        {
            Event eventEntity = default(Event);
            string queryString = "SELECT * FROM dbo.Events WHERE Id = @EventId " +
                                 "SELECT * FROM dbo.PurchasedTickets WHERE EventId = @EventId " +
                                 "SELECT * FROM dbo.ReservedTickets WHERE EventId = @EventId;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EventId", id));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader()) 
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        eventEntity = new Event();
                        eventEntity.Id = new Guid(reader["Id"].ToString());
                        eventEntity.Name = reader["Name"].ToString();
                        eventEntity.Allocation = int.Parse(reader["Allocation"].ToString());
                        eventEntity.TicketPurchases = new List<TicketPurchase>();
                        eventEntity.TicketReservations = new List<TicketReservation>();
                        if (reader.NextResult())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TicketPurchase ticketPurchase = new TicketPurchase();
                                    ticketPurchase.Id = new Guid(reader["Id"].ToString());
                                    ticketPurchase.Event = eventEntity;
                                    ticketPurchase.TicketQuantity = int.Parse(reader["TicketQuantity"].ToString());
                                    eventEntity.TicketPurchases.Add(ticketPurchase);
                                }
                            }
                        }

                        if (reader.NextResult())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TicketReservation ticketReservation = new TicketReservation();
                                    ticketReservation.Id = new Guid(reader["Id"].ToString());
                                    ticketReservation.Event = eventEntity;
                                    ticketReservation.ExpiryTime = DateTime.Parse(reader["ExpiryTime"].ToString());
                                    ticketReservation.TicketQuantity = int.Parse(reader["TicketQuantity"].ToString());
                                    ticketReservation.HasBeenRedeemed = bool.Parse(reader["HasBeenRedeemed"].ToString());
                                    eventEntity.TicketReservations.Add(ticketReservation);
                                }
                            }
                        }
                    }
                }
            }

            return eventEntity;
        }

        public void Save(Event eventEntity)
        {
            RemovePurchasedAndReservedTicketsFrom(eventEntity);

            InsertPurchasedTicketsFrom(eventEntity);
            InsertReservedTicketsFrom(eventEntity);
        }

        public void InsertReservedTicketsFrom(Event Event)
        {
            string insertSQL = "INSERT INTO ReservedTickets " +
                               "(Id, EventId, TicketQuantity, ExpiryTime, HasBeenRedeemed) " +
                               "VALUES " +
                               "(@Id, @EventId, @TicketQuantity, @ExpiryTime, @HasBeenRedeemed);";

            foreach (TicketReservation ticket in Event.TicketReservations)
            {
                using (SqlConnection connection =
                      new SqlConnection(connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSQL;

                    SqlParameter Idparam = new SqlParameter("@Id", ticket.Id.ToString());
                    command.Parameters.Add(Idparam);

                    SqlParameter EventIdparam = new SqlParameter("@EventId", ticket.Event.Id.ToString());
                    command.Parameters.Add(EventIdparam);

                    SqlParameter TktQtyparam = new SqlParameter("@TicketQuantity", ticket.TicketQuantity);
                    command.Parameters.Add(TktQtyparam);

                    SqlParameter Expiryparam = new SqlParameter("@ExpiryTime", ticket.ExpiryTime);
                    command.Parameters.Add(Expiryparam);

                    SqlParameter HasBeenRedeemedparam = new SqlParameter("@HasBeenRedeemed", ticket.HasBeenRedeemed);
                    command.Parameters.Add(HasBeenRedeemedparam);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

        }

        public void InsertPurchasedTicketsFrom(Event Event)
        {
            string insertSQL = "INSERT INTO PurchasedTickets " +
                               "(Id, EventId, TicketQuantity) " +
                               "VALUES " +
                               "(@Id, @EventId, @TicketQuantity);";

            foreach (TicketPurchase ticket in Event.TicketPurchases)
            {
                using (SqlConnection connection =
                      new SqlConnection(connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSQL;

                    SqlParameter Idparam = new SqlParameter("@Id", ticket.Id.ToString());
                    command.Parameters.Add(Idparam);

                    SqlParameter EventIdparam = new SqlParameter("@EventId", ticket.Event.Id.ToString());
                    command.Parameters.Add(EventIdparam);

                    SqlParameter TktQtyparam = new SqlParameter("@TicketQuantity", ticket.TicketQuantity);
                    command.Parameters.Add(TktQtyparam);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemovePurchasedAndReservedTicketsFrom(Event Event)
        {
            string deleteSQL = "DELETE PurchasedTickets WHERE EventId = @EventId; " +
                               "DELETE ReservedTickets WHERE EventId = @EventId;";

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = deleteSQL;

                SqlParameter Idparam = new SqlParameter("@EventId", Event.Id.ToString());
                command.Parameters.Add(Idparam);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
