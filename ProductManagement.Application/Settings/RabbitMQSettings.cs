namespace ProductManagement.Application.Settings;

public class RabbitMQSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public ExchangeSettings Exchanges { get; set; } = new();
    public QueueSettings Queues { get; set; } = new();
    public RoutingKeySettings RoutingKeys { get; set; } = new();
}

public class ExchangeSettings
{
    public string CartEvents { get; set; } = "cart.events";
    public string OrderEvents { get; set; } = "order.events";
}

public class QueueSettings
{
    public string OrderCreation { get; set; } = "order.creation.queue";
    public string StockDeduction { get; set; } = "stock.deduction.queue";
    public string CartClearing { get; set; } = "cart.clearing.queue";
}

public class RoutingKeySettings
{
    public string CartCheckedOut { get; set; } = "cart.checkedout";
    public string OrderCreated { get; set; } = "order.created";
}