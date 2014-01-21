//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Configuration.Provider;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web.Security;
//using CarDealerProject.Models.Nhibernate;

//namespace CarDealerProject.Models.Security
//{

////////TODO LATER
//    //TODO USE OF NHIBERNATE PROVIDER
//    public class NHRoleProvider : RoleProvider//, IRoleProvider
//    {
//        private readonly INHibernateSession nHibernateSession;
//        private string applicationName;

//        //TODO Paramter form default mapper DI
//        public NHRoleProvider()
//        {
//            this.nHibernateSession = new NHibernateSession();
//            applicationName = "CarDealer"; // TODO Get reflection
//        }

//        public override bool IsUserInRole(string username, string roleName)
//        {
//            var user = GetAllUser().FirstOrDefault(x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase));

//            if (user != null)
//            {
//                var actualRole = GetAllRole().FirstOrDefault(x => x.UserId == user.Id &&
//                                                                    x.RoleName.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

//                return (actualRole != null);
//            }
//            #region Comment

//            //    var users = nHibernateSession.GetAll<User>().ToList();

//            //    var user = users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase));
//            //    if (user != null)
//            //    {
//            //        //HACK Return alwalys true

//            //        var myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbContext"].ConnectionString);

//            //        try
//            //        {
//            //            myConnection.Open();

//            //            SqlDataReader myReader = null;
//            //            var myCommand = new SqlCommand("select * from Role", myConnection);

//            //            var roles = new List<Role>();

//            //            myReader = myCommand.ExecuteReader();
//            //            while (myReader.Read())
//            //            {
//            //                roles.Add(new Role()
//            //                {
//            //                    RoleName = myReader["RoleName"].ToString(),
//            //                    UserId = Convert.ToInt32(myReader["UserId"].ToString())
//            //                });
//            //            }

//            //            myConnection.Close();


//            //            if (roles.FirstOrDefault(x => x.UserId == user.Id) != null)
//            //                return true;
//            //        }
//            //        catch (Exception)
//            //        {

//            //            throw;
//            //        }

//            //        #region MyRegion

//            //        //using (var session = nHibernateSession.OpenSession(userClassName))
//            //        //{


//            //        //    User user_ = null;
//            //        //    var jobsWithoutLogs = session.QueryOver(() => user_)
//            //        //        .WithSubquery.WhereExists(QueryOver.Of<Role>()
//            //        //            .Where(log => log.UserId == user_.Id)
//            //        //            .Select(Projections.Id()))
//            //        //        .List();

//            //        //    //IQueryOver<User, Role> catQuery =
//            //        //    //    session.QueryOver<User>()
//            //        //    //           .JoinQueryOver(c => c.Roles);




//            //        //    var k = 2;

//            //        //    //var result = session.CreateCriteria(typeof(User), "u")
//            //        //    //                    .CreateAlias("Role", "r", NHibernate.SqlCommand.JoinType.InnerJoin)
//            //        //    //                    .Add(Restrictions.Disjunction()
//            //        //    //                    .Add(Restrictions.Like("u.Id", "r.UserId", MatchMode.Exact))
//            //        //    //                    .Add(Restrictions.Like("u.UserName", user.UserName, MatchMode.Exact)))
//            //        //    //                    .List<Role>();

//            //        //    //if (result != null)
//            //        //    //    return false;
//            //        //} 

//            //        #endregion
//            //    }
//            //}
//            //catch (Exception)
//            //{
//            //    ;
//            //} 
//            #endregion

//            return false;
//        }

//        public override string[] GetRolesForUser(string username)
//        {
//            if (string.IsNullOrEmpty(username))
//                throw new ProviderException("User name cannot be empty or null.");


//            string tmpRoleNames = "";

//            var myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbContext"].ConnectionString);

//            SqlCommand cmd = new SqlCommand("SELECT Rolename FROM UsersInRoles " +
//                                              " WHERE Username = @Username AND ApplicationName = @ApplicationName", myConnection);

//            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
//            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

//            SqlDataReader reader = null;

//            try
//            {
//                myConnection.Open();

//                reader = cmd.ExecuteReader();

//                while (reader.Read())
//                {
//                    tmpRoleNames += reader.GetString(0) + ",";
//                }
//            }
//            catch (SqlException)
//            {
//                // Handle exception.
//            }
//            finally
//            {
//                if (reader != null) { reader.Close(); }
//                myConnection.Close();
//            }

//            if (tmpRoleNames.Length > 0)
//            {
//                // Remove trailing comma.
//                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
//                return tmpRoleNames.Split(',');
//            }

//            return new string[0];






















//            var user = GetAllUser().FirstOrDefault(x => x.UserName == username);
//            if (user == null)
//                return new string[0];

//            var actualRoles = GetAllRole().Where(x => x.UserId == user.Id);

//            return actualRoles.Select(x => x.RoleName).ToArray();
//        }

//        public override void CreateRole(string roleName)
//        {
//            if (string.IsNullOrEmpty(roleName))
//                throw new ProviderException("Role name cannot be empty or null.");
//            if (roleName.Contains(","))
//                throw new ArgumentException("Role names cannot contain commas.");
//            if (RoleExists(roleName))
//                throw new ProviderException("Role name already exists.");
//            if (roleName.Length > 255)
//                throw new ProviderException("Role name cannot exceed 255 characters.");

//            var myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbContext"].ConnectionString);

//            try
//            {
//                var myCommand = myConnection.CreateCommand();

//                myCommand.CommandText = @"INSERT INTO role (RoleName) VALUES (@RoleName)";
//                myCommand.Parameters.Add("@RoleName", SqlDbType.VarChar);
//                myCommand.Parameters["@RoleName"].Value = roleName;

//                myConnection.Open();
//                myCommand.ExecuteNonQuery();
//                myConnection.Close();
//            }
//            catch (Exception ex)
//            {
//                Logger.Logger.LogError("An error occured during the 'AddUsersToRoles' procedure. Error Message:", ex);
//            }
//        }

//        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
//        {
//            var myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbContext"].ConnectionString);

//            var activeRoles = GetAllRole().Where(x => x.RoleName.Equals(roleName, StringComparison.InvariantCultureIgnoreCase)).ToList();

//            if (activeRoles.Any(x => x.UserId != 0)) 
//                throw new ProviderException("Cannot delete a populated role.");

//            if (!activeRoles.Any()) 
//                throw new ProviderException("Role does not exist.");

//            try
//            {
//                var myCommand = new SqlCommand(@"DELETE FROM role WHERE conditionColumn='@conditionName'", myConnection);
//                myCommand.Parameters.Add("@conditionName", SqlDbType.VarChar);


//                var idList = string.Join(",", activeRoles.Select(x => x.ToString()).ToArray());
//                myCommand.Parameters["@conditionName"].Value = "Id IN " + idList;

//                myConnection.Open();
//                var result = myCommand.ExecuteNonQuery();
//                myConnection.Close();
//            }
//            catch (Exception ex)
//            {
//                Logger.Logger.LogError("An error occured during the 'RemoveUsersFromRoles' procedure. Error Message:", ex);
//                return false;
//            }

//            return true;
//        }

//        public override bool RoleExists(string roleName)
//        {
//            var role = GetAllRole().FirstOrDefault(x => x.RoleName.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));
//            return (role != null);
//        }

//        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
//        {
//            var activeUsers = GetAllUser().Where(x => usernames.Contains(x.UserName)).ToList();

//            var myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbContext"].ConnectionString);

//            try
//            {
//                var myCommand = myConnection.CreateCommand();

//                myConnection.Open();

//                foreach (var activeUser in activeUsers)
//                {
//                    foreach (var roleName in roleNames)
//                    {
//                        myCommand.CommandText = @"INSERT INTO role (RoleName, UserId) VALUES (@RoleName, @UserId)";
//                        myCommand.Parameters.Add("@RoleName", SqlDbType.VarChar);
//                        myCommand.Parameters["@RoleName"].Value = roleName;

//                        myCommand.Parameters.Add("@UserId", SqlDbType.VarChar);
//                        myCommand.Parameters["@UserId"].Value = activeUser.Id;
//                        myCommand.ExecuteNonQuery();
//                    }
//                }

//                myConnection.Close();
//            }
//            catch (Exception ex)
//            {
//                Logger.Logger.LogError("An error occured during the 'AddUsersToRoles' procedure. Error Message:", ex);
//            }
//        }

//        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
//        {
//            var activeUsers = GetAllUser().Where(x => usernames.Contains(x.UserName));

//            var myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbContext"].ConnectionString);

//            try
//            {
//                var myCommand = new SqlCommand(@"DELETE FROM role WHERE conditionColumn='@conditionName'", myConnection);

//                myCommand.Parameters.Add("@conditionName", SqlDbType.VarChar);

//                var idList = string.Join(",", activeUsers.Select(x => x.ToString()).ToArray());
//                myCommand.Parameters["@conditionName"].Value = "Id IN " + idList;

//                myConnection.Open();
//                var result = myCommand.ExecuteNonQuery();
//                myConnection.Close();
//            }
//            catch (Exception ex)
//            {
//                Logger.Logger.LogError("An error occured during the 'RemoveUsersFromRoles' procedure. Error Message:", ex);
//            }
//        }

//        public override string[] GetUsersInRole(string roleName)
//        {
//            var role = GetAllRole().FirstOrDefault(x => x.RoleName.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

//            if (role == null)
//                return new string[0];

//            var actaulUsers = GetAllUser().Where(x => x.Id == role.UserId);

//            return actaulUsers.Select(x => x.UserName).ToArray();
//        }

//        public override string[] GetAllRoles()
//        {
//            var roles = GetAllRole();

//            return roles.Select(a => a.RoleName).ToArray();
//        }

//        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
//        {
//            var usersInRole = GetUsersInRole(roleName).ToList();

//            var filteredUsers = usersInRole.Where(x => x.Contains(usernameToMatch));
//            return filteredUsers.ToArray();
//        }

//        //TODO Appication name doesnt matter , yet TODO
//        public override string ApplicationName
//        {
//            get { return applicationName; }
//            set { applicationName = value; }
//        }

//        private IEnumerable<Role> GetAllRole()
//        {
//            var roles = new List<Role>();
//            var myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbContext"].ConnectionString);

//            try
//            {
//                myConnection.Open();

//                SqlDataReader myReader = null;
//                var myCommand = new SqlCommand("select * from Role", myConnection);


//                myReader = myCommand.ExecuteReader();
//                while (myReader.Read())
//                {
//                    roles.Add(new Role()
//                    {
//                        RoleName = myReader["RoleName"].ToString(),
//                        UserId = Convert.ToInt32(myReader["UserId"].ToString())
//                    });
//                }

//                myConnection.Close();
//            }
//            catch (Exception ex)
//            {
//                Logger.Logger.LogError("An error occured during the 'GetAllRoles' procedure. Error Message:", ex);
//            }

//            return roles;
//        }

//        private IEnumerable<User> GetAllUser()
//        {
//            var users = new List<User>();
//            var myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbContext"].ConnectionString);

//            SqlDataReader myReader = null;
//            var myCommand = new SqlCommand("SELECT * FROM UserProfile", myConnection);

//            try
//            {
//                myConnection.Open();

//                myReader = myCommand.ExecuteReader();
//                while (myReader.Read())
//                {
//                    users.Add(new User()
//                        {
//                            UserName = myReader["UserName"].ToString(),
//                            DisplayName = myReader["DisplayName"].ToString(),
//                            Password = myReader["Password"].ToString(),
//                            PasswordSalt = myReader["PasswordSalt"].ToString(),
//                        });
//                }
//            }
//            catch (Exception ex)
//            {
//                Logger.Logger.LogError("An error occured during the 'GetAllUser' procedure. Error Message:", ex);
//            }
//            finally
//            {
//                myConnection.Close();
//            }

//            return users;
//        }
//    }

//    //public interface IRoleProvider
//    //{
//    //}
//}