using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace eCommerce.Infrastructure.Presistense.Migrations;
public class DataBaseMigrator(DapperDbContext _context)
{
    public async Task CreateTablesAsync()
    {
        var createScript = @"            
            CREATE TABLE IF NOT EXISTS users (
                id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
                email VARCHAR(256),
                password TEXT,
                name VARCHAR(100),
                gender VARCHAR(20),
                createdat TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                updatedat TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
            );

            CREATE TABLE IF NOT EXISTS roles (
                id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
                name VARCHAR(50),
                createdat TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                updatedat TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
            );

            CREATE TABLE IF NOT EXISTS user_roles (
                userid UUID NOT NULL,
                roleid UUID NOT NULL,
                createdat TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                
                CONSTRAINT pk_user_roles PRIMARY KEY (userid, roleid),
                CONSTRAINT fk_userroles_users FOREIGN KEY (userid) 
                    REFERENCES users(id) ON DELETE CASCADE,
                CONSTRAINT fk_user_roles_role FOREIGN KEY (roleid) 
                    REFERENCES roles(id) ON DELETE CASCADE
            );
            
            CREATE INDEX IF NOT EXISTS ix_users_email ON users(email);
            CREATE INDEX IF NOT EXISTS ix_role_name ON roles(name);
            CREATE INDEX IF NOT EXISTS ix_user_roles_userid ON user_roles(userid);
            CREATE INDEX IF NOT EXISTS ix_user_roles_roleid ON user_roles(roleid);

            CREATE OR REPLACE FUNCTION update_updatedat_column()
            RETURNS TRIGGER AS $$
            BEGIN
                NEW.updatedat = NOW();
                RETURN NEW;
            END;
            $$ language 'plpgsql';

            DROP TRIGGER IF EXISTS update_users_updatedat ON users;
            CREATE TRIGGER update_users_updatedat 
                BEFORE UPDATE ON users
                FOR EACH ROW EXECUTE FUNCTION update_updatedat_column();

            DROP TRIGGER IF EXISTS update_roles_updatedat ON roles;
            CREATE TRIGGER update_roles_updatedat 
                BEFORE UPDATE ON roles 
                FOR EACH ROW EXECUTE FUNCTION update_updatedat_column();
        ";


        await _context.DbConnection.ExecuteAsync(createScript);
    }
    public async Task SeedDefaultDataAsync()
    {
        var seedScript = "INSERT INTO \"roles\" (name) VALUES " +
                         "('Administrator')," +
                         "('User')," +
                         "('Moderator') " +
                         "ON CONFLICT (name) DO NOTHING;";

        await _context.DbConnection.ExecuteAsync(seedScript);
    }
}
