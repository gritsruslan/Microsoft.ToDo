using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Application.Exceptions;
using Microsoft.ToDo.Application.Services;
using Microsoft.ToDo.Domain.Models;
using Moq;

namespace Microsoft.ToDo.UnitTests;

public class AuthServiceShould
{
    private readonly AuthService _authService;
    
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
    
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

    public AuthServiceShould()
    {
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);
        var jwtGeneratorMock = new Mock<IJwtGenerator>();
        
        var loginValidatorMock = new Mock<IValidator<LoginRequest>>();
        var registerValidatorMock = new Mock<IValidator<RegisterRequest>>();
        
        loginValidatorMock.Setup(v => v.ValidateAsync(
            It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        registerValidatorMock.Setup(v => v.ValidateAsync(
            It.IsAny<RegisterRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        
        _jwtGeneratorMock = jwtGeneratorMock;
        _userManagerMock = userManagerMock;
        
        _authService = new AuthService(
            userManagerMock.Object,
            jwtGeneratorMock.Object,
            loginValidatorMock.Object,
            registerValidatorMock.Object);
    }

    [Fact]
    public async Task ThrowExceptionWhileLogin_WhenUserDoesNotExist()
    {
        var email = "ThrowExceptionWhileLogin_WhenUserDoesNotExist@test.com";
        var request = new LoginRequest(email, string.Empty);
        
        _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(() => null);
        
        await _authService.Invoking(s => s.Login(request, CancellationToken.None))
            .Should().ThrowAsync<InvalidEmailOrPasswordException>();
    }
    
    [Fact]
    public async Task ThrowExceptionWhileLogin_WhenPasswordIsInvalid()
    {
        var email = "ThrowExceptionWhileLogin_WhenPasswordIsInvalid@test.com";
        var password = "StrongPassword_52";
        var user = new ApplicationUser() { Email = email };
        
        _userManagerMock.Setup(m =>
            m.FindByEmailAsync(email)).ReturnsAsync(() => user);
        
        _userManagerMock.Setup(m => m.CheckPasswordAsync(
            user, password)).ReturnsAsync(() => false);
        
        await _authService.Invoking(s => s.Login(new LoginRequest(email, password), CancellationToken.None))
            .Should().ThrowAsync<InvalidEmailOrPasswordException>();
    }
    
    [Fact]
    public async Task SuccessfullyLoginUser()
    {
        var email = "SuccessfullyLoginUser@test.com";
        var password = "SuccessfullyLoginUser_52";
        var user = new ApplicationUser { Email = email };
        var jwt = "jwt-token";
        
        _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(() => user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, password)).ReturnsAsync(() => true);
        
        _jwtGeneratorMock.Setup(g => g.GenerateAccessToken(
            It.Is<ApplicationUser>(u => u.Email == email))).Returns(jwt);
        
        var token = await _authService.Login(new LoginRequest(email, password), CancellationToken.None);
        
        _jwtGeneratorMock.Verify(g => g.GenerateAccessToken(
            It.Is<ApplicationUser>(u => u.Email == email)), Times.Once);
        token.Should().Be(jwt);
    }

    [Fact]
    public async Task ThrowDomainExceptionWhileRegister_IfFailedToCreateUser()
    {
        var email = "ThrowDomainExceptionWhileRegister_IfFailedToCreateUser@test.com";
        var password = "ThrowDomainExceptionWhileRegister_52";
        
        _userManagerMock.Setup(m => m.CreateAsync(
            It.Is<ApplicationUser>(u => u.Email == email), password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

        await _authService.Invoking(s => s.Register(
                new RegisterRequest(email, password), CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task SuccessfullyRegisterUser()
    {
        var email = "SuccessfullyRegisterUser@test.com";
        var password = "SuccessfullyRegisterUser_52";
        
        _userManagerMock.Setup(m => m.CreateAsync(
            It.Is<ApplicationUser>(u => u.Email == email), password)).ReturnsAsync(IdentityResult.Success);
        
        await _authService.Register(new RegisterRequest(email, password), CancellationToken.None);
        
        _userManagerMock.Verify(m => m.CreateAsync(
            It.Is<ApplicationUser>(u => u.Email == email), password), Times.Once);
        _jwtGeneratorMock.Verify(g => 
            g.GenerateAccessToken(It.Is<ApplicationUser>(u => u.Email == email)), Times.Once);
    }
    
    [Fact]
    public async Task ThrowUnauthorizedExceptionWhileGetMe_IfUserIsNotAuthenticated()
    {
        await _authService.Invoking(s => s.GetMe(null, CancellationToken.None))
            .Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task ThrowUnauthorizedExceptionWhileGetMe_WhenUserWasNotFound()
    {
        var userId = "ThrowUnauthorizedExceptionWhileGetMe_WhenUserWasNotFound";
        
        _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(() => null);
        
        await _authService.Invoking(s => s.GetMe(userId, CancellationToken.None))
            .Should().ThrowAsync<UnauthorizedException>();
    }
    
    [Fact]
    public async Task SuccessfullyGetMe()
    {
        var userId = "SuccessfullyGetMe";
        var email = "SuccessfullyGetMe@test.com";
        var user = new ApplicationUser { Id = userId, Email = email};
        
        _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(() => user);
        
        var resultUser = await _authService.GetMe(userId, CancellationToken.None);
        
        resultUser.Should().BeEquivalentTo(new GetMeResponse(userId, email));
    }
}