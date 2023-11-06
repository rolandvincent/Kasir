> Project Status : **Under Development**

# Prototype

[Figma](https://www.figma.com/proto/ILIZ4OGPJhRXcHfpUSh1IZ/Untitled?type=design&node-id=14-1169&t=a6WPHoOc4rHClt5O-1&scaling=min-zoom&page-id=0%3A1&starting-point-node-id=14%3A1169&mode=design)

# Screenshoots

![HOME](https://github.com/rolandvincent/Kasir/assets/52077393/65f56c1a-f8cf-45c9-9596-c40381c3e148)

![PRODUCTS](https://github.com/rolandvincent/Kasir/assets/52077393/1277565e-9818-49ee-8350-9e1b415cb961)

![ADD_PRODUCT](https://github.com/rolandvincent/Kasir/assets/52077393/9ef38cea-69b7-4b8d-8e5e-6adc078d2650)

![BARCODE_SCAN](https://github.com/rolandvincent/Kasir/assets/52077393/5d3411ac-801d-47dc-9c99-9270355456d5)

![TRANSACTION_WINDOW](https://github.com/rolandvincent/Kasir/assets/52077393/7ae6d8bc-7005-48c9-8a88-c962ba8c51fb)

*And many more...*

# How to Use
First open config file e.g. Kasir.dll.config or Kasir.exe.config. If not exist, create new one.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="connectionStrings" value="server=localhost;user=root;password=;database=iCassierDB" />
		<add key="DatabaseType" value="MySQL" />
	</appSettings>
</configuration>
```

Change connectionStrings to your connection string. You can change databaseType to MySQL, SQLServer or SQLite
