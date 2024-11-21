//https://isc.devexpress.com/internal/ticket/list?configuration={%22AppliedFilters%22:[{%22filterName%22:%22PlatformedProductId%22,%22selectedValues%22:[%22d76afe22-512e-45a3-ad81-7b245352e111:e2bf2f68-d2cb-41c8-b8f3-29c8d66835e9%22,%22d76afe22-512e-45a3-ad81-7b245352e111:4172fd27-cf4e-4275-8713-858656d21847%22,%22d76afe22-512e-45a3-ad81-7b245352e111:82117e67-bb10-4276-849d-a418aaa07bc2%22,%22d76afe22-512e-45a3-ad81-7b245352e111:17d5a572-0e55-44b1-82d3-4ab34cc06a20%22,%22d76afe22-512e-45a3-ad81-7b245352e111:63c9258e-8f15-4141-8615-a20fd6ccd03b%22,%22d76afe22-512e-45a3-ad81-7b245352e111:9eda7be2-8f23-467b-bb4e-9d546db79c87%22]},{%22filterName%22:%22ControlId%22,%22selectedValues%22:[%22da5d0e9f-4f67-4431-86c2-cfb1b25620b7%22]},{%22filterName%22:%22FeatureId%22,%22selectedValues%22:[%224c925dff-5491-4686-9f60-7771b06b2595%22]}]}

let ll;

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function openTickets() {
	ll=document.getElementsByClassName('ticket-id text-center');
	 for (let i = 0; i < ll.length; i++) {
       // console.log(`Waiting ${i} seconds...`);
		let x=ll[i];
	    let fullUrl='https://isc.devexpress.com/internal/ticket/details/'+x.innerText;
		window.open(fullUrl, '_blank')
		console.log(fullUrl);
		await sleep(500);
    }

}

openTickets();
