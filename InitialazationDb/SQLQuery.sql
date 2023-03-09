SET IDENTITY_INSERT pfe.dbo.Questions ON;

	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (1,'How old are you ?','age')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (2,'What is your annual income before taxes ?','income')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (3,'When do you plan your retirement?','retirement')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (4,'What is your current personal or familial financial situation?','situation')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (5,'What is your main investment objective?','objective')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (6,'How do you rate your knowledge about investment?','knowledge')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (7,'How many different investment products (shares, funds, bonds, certificats) did you hold within the last year?','products')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (8,'Assuming the markets are having a difficult time, what decline in the value of your investments could you tolerate?','tolerance')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (9,'If you put 1000$ for a year, what range of potential gain or loss would be most acceptable?','potential')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (10,'Your investments are down by 20% in value over a-one year period, what would you do?','period')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (11,'When managing your financial investments, would you describe yourself as someone who looks for :','description')
	insert into pfe.dbo.Questions("Id_question","Quest","Code_question") values (12,'When you think of the word “risk”, which of the following words comes to mind first?','risk')


SET IDENTITY_INSERT pfe.dbo.Questions OFF;
SET IDENTITY_INSERT pfe.dbo.Choices ON;

	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (1,1,1,'More than 70 years')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (2,1,2,'Between 61 and 70 years')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (3,1,5,'Between 51 and 60 years')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (4,1,10,'Between 41 and 50 years')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (5,1,15,'Between 18 and 40 years')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (6,2,1,'Less than 25,00$')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (7,2,2,'Between 25,00$ and 50,00$')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (8,2,5,'Between 50,00$ and 75,00$')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (9,2,10,'Between 75,00$ and 100,00$')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (10,2,15,'More than 100,00$')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (11,3,1,'In less than 3 years')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (12,3,2,'In 3 to 5 years')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (13,3,5,'In 6 to 12 years')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (14,3,10,'In 13 to 20 years')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (15,3,15,'In more than 20 years')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (16,4,1,'Precarious : Highly indebted and few savings')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (17,4,2,'Quite precarious : Rather indebted and few savings')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (18,4,5,'Quite stable : Some debts and average savings')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (19,4,10,'Good : Few debts and average savings')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (20,4,15,'Very good : Barely or no debt and a lot of savings')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (21,5,1,'Security : I want that these investments will be  100% safe even if it means they will not keep up with inflation')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (22,5,2,'Protection against inflation : I’m uncomfortable with yield fluctuations, but I’m willing to accept a low level of fluctuation to keep up with inflations')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (23,5,5,'Development and Security : I’m looking for a balance between security and development to get a return slightly above inflation')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (24,5,10,'Development: It’s mainly growth that interest me and I’m less concerned about the  fluctuation of return')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (25,5,15,'Maximum growth : My only goal is maximum growth and I’m not concerned about the fluctuation of return')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (26,6,1,'Novice : Limited investment knowledge')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (27,6,2,'Beginner : I know that there is different type of investment but i don’t know what is the difference between them')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (28,6,5,'Good : I know the characteristics of shares and bonds as well as how they differ in terms of  volatility')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (29,6,10,'Very good : I have a solid understanding about the different type of investment and their associated risk')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (30,6,15,'Excellent : I have a deep understanding of the different type of investment and strategies, risks and their relation to the market volatility')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (31,7,1,'0')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (32,7,5,'1 to 5')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (33,7,9,'6 to 10 ')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (34,7,15,'More than 10')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (35,8,1,'No drop')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (36,8,2,'Less than 5%')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (37,8,5,'From 5% to 10%')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (38,8,10,'From 10% to 20%')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (39,8,15,'More than 20%')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (40,9,1,'From - 100$ to + 300$')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (41,9,2,'From - 500$ to + 1,000$')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (42,9,5,'From - 1,00$ to  + 1,50$')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (43,9,10,'From - 1,50$ to + 2,00$')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (44,9,15,'From - 2,00$ to + 2,50$')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (45,10,1,'I would sell my investment even if it resulted in an immediate loss')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (46,10,5,'I would hold my investment until it returns to its original value and then transfer it to a less volatile investment')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (47,10,9,'I would keep my investment because we expect market fluctuations. It’s the long-growth of this investment that interests me and the fluctuations in short term do  not worry me ')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (48,10,15,'I would invest additional sums in the investment. This would be an ideal opportunity to acquire stocks at a better price, therefore, improve the long-term performance of my portfolio')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (49,11,1,'Low returns, without any risk of losing your capital ')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (50,11,5,'A reasonable return, with a good degree of security for your invested capital')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (51,11,9,'A good return, with reasonable security for your invested capital')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (52,11,15,'Very high returns, regardless of a high risk of losing part of your capital')


	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (53,12,1,'Loss')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (54,12,5,'Uncertainty')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (55,12,9,'Opportunity')
	insert into pfe.dbo.Choices ("Id_choix","Id_question","Weight","Choix") values (56,12,15,'Thrill')


SET IDENTITY_INSERT pfe.dbo.Choices OFF;