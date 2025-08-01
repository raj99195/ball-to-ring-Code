using UnityEngine;
using System;
using GoogleMobileAds.Api;
public class admobLauncher : MonoBehaviour {
	private InterstitialAd interstitial;
	// Use this for initialization
	void Start() {
		//set ad id's
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/1033173712"; //test id
#elif UNITY_IPHONE
		string adUnitId = "//adUnitID_iOS";
#else
		string adUnitId = "unexpected_platform";
#endif

		//set app id
		MobileAds.Initialize("ca-app-pub-3940256099942544~3347511713"); // test id

		//run's app id(dont change this line!)
		this.interstitial = new InterstitialAd(adUnitId);

		sendRequest();

		//----------------------------------------------------------------------------------- start events part
		// Called when an ad request has successfully loaded.
		this.interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		this.interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
		//------------------------------------------------------------------------------------ ends events part

	}

	public void HandleOnAdLoaded(object sender, EventArgs args) {

		print("a ad is loaded");

	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {

		print("ad load is not done : " + args.Message);

	}

	public void HandleOnAdOpened(object sender, EventArgs args) {

		print("ad is successfully showed!");

	}

	public void HandleOnAdClosed(object sender, EventArgs args) {

		print("ad is successfully showed and its closed by user right now!");

	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args) {

		print("user is closed the app!");

	}

	public void sendRequest() {

		if (!this.interstitial.IsLoaded()) {

			//send a request for ad to admob server
			AdRequest request = new AdRequest.Builder().Build();

			//if the admob's server give a ad,it will saved into a variable
			this.interstitial.LoadAd(request);
		}

	}
	public void showads() {
		if (this.interstitial.IsLoaded()) {
			this.interstitial.Show();
		}
	}
}
