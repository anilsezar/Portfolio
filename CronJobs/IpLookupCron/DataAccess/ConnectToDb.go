package DataAccess

import (
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
	"os"
)

func DbContext() *gorm.DB {

	connectionString := connectionStringFromEnv()
	db, err := gorm.Open(postgres.Open(connectionString), &gorm.Config{Logger: logger.Default.LogMode(logger.Info)})
	if err != nil {
		panic("failed to connect to database")
	}

	return db
}

func connectionStringFromEnv() string {

	host, _ := os.LookupEnv("SQL_DB_HOST")
	port, _ := os.LookupEnv("SQL_DB_PORT")
	user, _ := os.LookupEnv("SQL_DB_USER")
	password, _ := os.LookupEnv("SQL_DB_PASSWORD")
	dbName, _ := os.LookupEnv("SQL_DB_NAME")
	sslMode, _ := os.LookupEnv("SQL_DB_SSL_MODE")
	timezone, _ := os.LookupEnv("SQL_DB_TIMEZONE")

	if len(host) == 0 || len(port) == 0 || len(user) == 0 || len(password) == 0 || len(dbName) == 0 || len(sslMode) == 0 || len(timezone) == 0 {
		panic("Failed to retrieve db connection string. ")
	}

	var connectionString = "host=" + host + " port=" + port + " user=" + user +
		" password=" + password + " dbname=" + dbName + " sslmode=" + sslMode +
		" TimeZone=" + timezone

	return connectionString
}
