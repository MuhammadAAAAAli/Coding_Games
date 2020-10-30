var recruiters = ["recruit", "hr", "culture", "talent a", "human r", "culture", "headhunting"];
var mode = 1;

window.scrollTo(0, 5000000);
var scrollUp = setInterval(function(){ window.scrollTo(0, 0); }, 10300);
var scrollDown = setInterval(function(){ window.scrollTo(0, 5000000); }, 500);
var counterMe =  10;
var autoScroll = true;
var totalCount = 0;

function containsAny(str, substrings) {
    for (var i = 0; i != substrings.length; i++) {
       var substring = substrings[i];
       if (str.indexOf(substring) != - 1) {
         return substring;
       }
    }
    return null; 
}

function KeepGrb(){
	counterMe++;
	if (autoScroll && totalCount > 2500)
	{
		clearInterval(scrollUp);
		clearInterval(scrollDown);
		autoScroll = false;
		console.log('auto-scroll has been cleared');
	}

	var connectionsOnPage = document.getElementsByClassName("discover-entity-card");
	totalCount = connectionsOnPage.length;
	if (counterMe < totalCount-1 && document.getElementsByClassName('artdeco-modal').length == 0)
	{	
		var allConnectionInfo = connectionsOnPage[counterMe].innerText.toLowerCase();
		var ocupationElement = connectionsOnPage[counterMe].getElementsByClassName("discover-person-card__occupation");
		var ocupation;
		if(ocupationElement.length > 0 )
		{
			ocupation = ocupationElement[0].innerText.toLowerCase();
		}
		else
		{
			ocupation = '';
		}

		if (containsAny(ocupation, mode == 1 ? recruiters : transporters))
		{
			connectionsOnPage[counterMe].getElementsByClassName("artdeco-button")[1].click();
			console.log('==========');
			console.log(allConnectionInfo)
			console.log('Done index '+counterMe+' out of '+ totalCount);
			console.log('==========');	

			setTimeout(function() {
				KeepGrb();
			}, 7000);	
		}
		else
		{	

			console.log('no ocupation match');
			console.log('Index '+counterMe+' out of '+ totalCount);		
			setTimeout(function() {
				KeepGrb();
			}, 200);				
		}
	}
	else
	{
		clearInterval(scrollUp);
		clearInterval(scrollDown);
		console.log('interval & auto-scroll has been cleared');
	}
}

function sanitizeInnerHtml(input)
{
	return input.replace('Connect','').replace(/(?:\r\n|\r|\n)/g, '').replace(' ','').replace(/ /g,"").replace('Invite','').replace('toconnect','');	
}

setTimeout(function() {
   KeepGrb();
}, 4000);
