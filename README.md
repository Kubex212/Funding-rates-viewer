# Funding rates table
This is a simple program that shows funding rates and predicted funding rates if the market provides such. Currently there are 4 markets implemented: Bitfinex, Phemex, Binance and Huobi.
# How to use
First you need to add App.config file to the main directory. It should contain Phemex' API ID and secret key. It should look like this:
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="phemexSecretKey" value="your_key"/>
		<add key="phemexId" value="your_id"/>
	</appSettings>
</configuration>
```
# About
My goal is to implement many more markets and provide many more functionalities. My work is in progress. Currently UI support only Polish language but this will change should the need arise.

