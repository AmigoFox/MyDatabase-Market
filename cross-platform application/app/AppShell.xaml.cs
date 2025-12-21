namespace app
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
            Routing.RegisterRoute("OrdersPage", typeof(OrdersPage));
            Routing.RegisterRoute("PaymentsPage", typeof(PaymentsPage));
        }
    }
}
