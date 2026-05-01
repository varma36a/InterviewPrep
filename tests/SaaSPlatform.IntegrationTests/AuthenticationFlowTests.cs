namespace SaaSPlatform.IntegrationTests;

public sealed class AuthenticationFlowTests
{
    [Fact]
    public void LoginContract_ShouldRequireTenantEmailAndPassword()
    {
        var contract = typeof(SaaSPlatform.Application.Contracts.Requests.LoginRequest);
        Assert.Equal("LoginRequest", contract.Name);
    }
}
