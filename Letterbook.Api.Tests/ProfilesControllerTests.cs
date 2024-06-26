﻿using Letterbook.Api.Controllers;
using Letterbook.Api.Dto;
using Letterbook.Api.Mappers;
using Letterbook.Core.Extensions;
using Letterbook.Core.Tests.Fakes;
using Medo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace Letterbook.Api.Tests;

public class ProfilesControllerTests : WithMockContext
{
	private readonly ITestOutputHelper _output;
	private readonly ProfilesController _controller;
	private readonly FakeProfile _fakeProfile;
	private readonly Models.Profile _profile;

	public ProfilesControllerTests(ITestOutputHelper output)
	{
		_output = output;
		_output.WriteLine($"Bogus seed: {Init.WithSeed()}");
		_controller = new ProfilesController(Mock.Of<ILogger<ProfilesController>>(), CoreOptionsMock, ProfileServiceMock.Object,
			new MappingConfigProvider(CoreOptionsMock), AuthorizationServiceMock.Object)
		{
			ControllerContext = new ControllerContext()
			{
				HttpContext = MockHttpContext.Object
			}
		};

		_fakeProfile = new FakeProfile(CoreOptionsMock.Value.BaseUri().Authority);
		_profile = _fakeProfile.Generate();
	}

	[Fact]
	public void Exists()
	{
		Assert.NotNull(_controller);
	}

	[Fact(DisplayName = "Should get a profile by ID")]
	public async Task CanGetProfile()
	{
		ProfileServiceAuthMock.Setup(m => m.LookupProfile(_profile.GetId())).ReturnsAsync(_profile);

		var result = await _controller.Get(_profile.GetId());

		var response = Assert.IsType<OkObjectResult>(result);
		var actual = Assert.IsType<FullProfileDto>(response.Value);
		Assert.Equal(_profile.Handle, actual.Handle);
	}

	[Fact(DisplayName = "Should create a profile owned by an actor")]
	public async Task CanCreateProfile()
	{
		var account = new FakeAccount().Generate();
		var profile = new FakeProfile(new Uri("https://letterbook.example/actor/new"), account).Generate();
		profile.Handle = "test_handle";
		ProfileServiceAuthMock.Setup(m => m.CreateProfile(account.Id, profile.Handle)).ReturnsAsync(profile);

		var result = await _controller.Create(account.Id, profile.Handle);

		var response = Assert.IsType<OkObjectResult>(result);
		var actual = Assert.IsType<FullProfileDto>(response.Value);
		Assert.Equal(profile.Handle, actual.Handle);
	}

	[Fact(DisplayName = "Should add a custom field to a profile")]
	public async Task CanAddField()
	{
		var expected = new Models.CustomField()
		{
			Label = "test label",
			Value = "test value"
		};
		ProfileServiceAuthMock.Setup(m => m.InsertCustomField(_profile.GetId(), 0, expected.Label, expected.Value))
			.ReturnsAsync(new Models.UpdateResponse<Models.Profile>()
			{
				Original = _profile,
				Updated = _profile
			});

		var result = await _controller.AddField(_profile.GetId(), 0, expected);

		var response = Assert.IsType<OkObjectResult>(result);
		var actual = Assert.IsType<FullProfileDto>(response.Value);
		Assert.Equal(_profile.Handle, actual.Handle);
	}

	[Fact(DisplayName = "Should remove a custom field from a profile")]
	public async Task CanRemoveField()
	{
		ProfileServiceAuthMock.Setup(m => m.RemoveCustomField(_profile.GetId(), 0))
			.ReturnsAsync(new Models.UpdateResponse<Models.Profile>()
			{
				Original = _profile,
				Updated = _profile
			});

		var result = await _controller.RemoveField(_profile.GetId(), 0);

		var response = Assert.IsType<OkObjectResult>(result);
		var actual = Assert.IsType<FullProfileDto>(response.Value);
		Assert.Equal(_profile.Handle, actual.Handle);
	}

	[Fact(DisplayName = "Should update a custom field on a profile")]
	public async Task CanUpdateField()
	{
		var expected = new Models.CustomField()
		{
			Label = "new label",
			Value = "new value"
		};
		ProfileServiceAuthMock.Setup(m => m.UpdateCustomField(_profile.GetId(), 0, expected.Label, expected.Value))
			.ReturnsAsync(new Models.UpdateResponse<Models.Profile>()
			{
				Original = _profile,
				Updated = _profile
			});

		var result = await _controller.UpdateField(_profile.GetId(), 0, expected);

		var response = Assert.IsType<OkObjectResult>(result);
		var actual = Assert.IsType<FullProfileDto>(response.Value);
		Assert.Equal(_profile.Handle, actual.Handle);
	}
}