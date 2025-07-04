﻿using BudgetBuddy.Application.Account.Models;
using BudgetBuddy.Infrastructure.Services.Serialization;
using MediatR;

namespace BudgetBuddy.Application.Account.Queries;

public class GetUserQuery : IRequest<GetUserResult>
{
    internal class Handler(IJsonSerializer serializer) : IRequestHandler<GetUserQuery, GetUserResult>
    {
        public async Task<GetUserResult> Handle(GetUserQuery query, CancellationToken cancellationToken = default)
        {
            var authenticationToken = await SecureStorage.Default.GetAsync("authentication_token");
            if (string.IsNullOrWhiteSpace(authenticationToken))
                return new GetUserResult { IsAuthenticated = false };

            var json = await SecureStorage.GetAsync("authentication_user");
            if (string.IsNullOrWhiteSpace(json))
                return new GetUserResult { IsAuthenticated = false };

            var accountModel = serializer.Deserialize<AccountModel>(json);
            if (accountModel == null)
                return new GetUserResult { IsAuthenticated = false };

            return new GetUserResult
            {
                Id = accountModel.Id,
                FirstName = accountModel.FirstName,
                LastName = accountModel.LastName,
                EmailAddress = accountModel.EmailAddress,
                AuthenticationToken = authenticationToken,
                IsAuthenticated = true
            };
        }
    }
}