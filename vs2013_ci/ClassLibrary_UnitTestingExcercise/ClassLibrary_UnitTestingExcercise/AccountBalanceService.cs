  1  ﻿using System;
  2  using System.Collections.Generic;
  3  using System.Linq;
  4  using System.Text;
  5  using BusinessObjects.Entities;
  6  using System.Security;
  7  
  8  namespace BusinessObjects.Services
  9  {
 10      public class AccountBalanceService
 11      {
 12          private IAverageJoeBankService _averageJoeService;
 13  
 14          public AccountBalanceService(IAverageJoeBankService averageJoeService)
 15          {
 16              _averageJoeService = averageJoeService;
 17          }
 18  
 19          public double GetAccountBalanceByUser(User user)
 20          {       
 21              // the authenticate method below takes too much time! 
 22              bool isAuthenticated = _averageJoeService.Authenticate(user.UserName, user.Password);
 23  
 24              if (!isAuthenticated)
 25                  throw new SecurityException("User is not authenticated"); 
 26              
 27              // access database using username and get the balance 
 28  
 29              return 100;      
 30          }
 31  
 32      
 33  }