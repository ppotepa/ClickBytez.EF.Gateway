namespace ClickBytez.EF.Gateway.Core.Binders
{
    public class GenericActionBinder
    {
        //private readonly IConfiguration configuration;
        //private readonly GatewayConfiguration gatewayConfiguration;
        //private readonly DbContext dbContext;

        //public GenericActionBinder(IConfiguration configuration, DbContext dbContext)
        //{
        //    this.configuration = configuration;
        //    this.gatewayConfiguration = configuration.GetGatewayConfiguration();
        //    this.dbContext = dbContext;
        //}
        

        //public Task BindModelAsync(ModelBindingContext bindingContext)
        //{
        //    string actionTypeString = bindingContext.ValueProvider.GetValue("type").ToString();
        //    string entityTypeJson = bindingContext.ValueProvider.GetValue("entity").ToString();

        //    if (string.IsNullOrEmpty(actionTypeString))
        //    {
        //        throw new InvalidOperationException("Type cannot be empty.");
        //    }

        //    string[] split = actionTypeString.Split(".");

        //    if (split.Length != 2)
        //    {
        //        throw new InvalidOperationException("Invalid actionType String");
        //    }

        //    string actionType = split[0];
        //    string entityName = split[1];

        //    Type targetEntity = dbContext.Model
        //                            .GetEntityTypes()
        //                            .First(type => type.ClrType.Name.Equals(entityName, StringComparison.InvariantCultureIgnoreCase))
        //                            .ClrType;

        //    ActionType targetActionType = Enum.Parse<ActionType>(actionType, true);
        //    IAction<IEntity> instance = ActionBase.CreateInstance(targetEntity, targetActionType);
        //    bindingContext.Result = ModelBindingResult.Success(instance);

        //    return Task.CompletedTask;
        //}
    }
}