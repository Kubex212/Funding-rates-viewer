# Funding rates table
This is a simple program that shows funding rates and predicted funding rates if the market provides such. Currently there are 5 markets implemented: Bitfinex, Phemex, Binance, Huobi and FTX.
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
After building, clicking the button and waiting for data to load you should see 
![The table](https://i.postimg.cc/597Rxv90/tabelka.png)
# About
My goal is to implement many more markets and provide many more functionalities. My work is in progress. Currently UI support only Polish language but this will change should the need arise.

