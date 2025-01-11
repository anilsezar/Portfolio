package main

import (
	"fmt"
	"github.com/joho/godotenv"
	"strings"
	"time"
)

func main() {
	if err := godotenv.Load(); err != nil {
		fmt.Println("Env file not present")
	}

	var db = DbContext()

	var requests []Request
	result := db.Where("country = ?", "").Find(&requests)
	if result.Error != nil {
		panic("failed to query requests")
	}

	deletedRowCount := 0
	for _, req := range requests {
		if req.ClientIP == "127.0.0.1" ||
			req.ClientIP == "31.223.32.192" ||
			strings.Contains(strings.ToLower(req.UserAgent), "uptimerobot") {
			fmt.Println("Will delete this row: " + fmt.Sprintf("%v", req))
			db.Delete(&req)
			deletedRowCount++
			continue
		}

		ipInfo, err := GetIPInfo(req.ClientIP)
		if err != nil {
			time.Sleep(time.Second * 10)
			fmt.Println(err)
			continue
		}

		req.Country = GetFlag(ipInfo.CountryCode) + " - " + ipInfo.Country
		req.City = ipInfo.City

		db.Save(&req)
		time.Sleep(time.Second * 2) // Rate limit the request
	}

	fmt.Println("ip-lookup-cron is done.")
	fmt.Println("Affected: " + fmt.Sprintf("%d", result.RowsAffected) + " rows")
	fmt.Println("Deleted: " + fmt.Sprintf("%d", deletedRowCount) + " rows")
}
