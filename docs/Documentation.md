### Working with JSON
JSON Source component has been developed in order to simplify and speed up the package development when dealing with JSON Data as input. It uses the amazing json.net library and provide convenient UI in order to achieve several possible goals. Although it is not strictly mandatory to be a master of JSON, it is really recommended to learn it first. Knowing JSON may save you a lot of time when dealing with empty output or errors. In case you have to deal with JSON but you have no idea where to start, have a look at the W3C tutorials (here: [http://www.w3schools.com/json/](http://www.w3schools.com/json/)).

## Basics:
### JsonSource and JsonFilter
Once installed, JSONSource will expose two plugins: JSONFilter and JSONSource. The first is used to parse json strings into SSIS columns, while the second component will act as a real datasource, retrieving the data elsewhere. 
As you might understand, JSONFilter requires at least one input lane to be attached. Once attached, the user can then choose which column has to be parsed and how to handle parsed data (see IO Mapping section for more details). For each row passing through the filter, data will be outputted into the output lane, parsed as configured by the user. 
JSONSource acts differently. In fact, it does not require a mandatory input lane, and its main goal is to retrieve json data from an external sources. What you should stick in your mind is the following: JSONSource has to be used when willing to retrieve the JSON data from an external source, such as web-service or a file on the disk; JSON Filter is useful when there already is some json data coded into a rowset. JsonSource retrieves data and parses it, while JsonFilter only parses input data. 

### Configuration
JsonSource isn't that complex once you understand how to use it. The UI has been developed in order to make the configuration process easy and straight forward. In order to use correctly the component, you should set up the following settings.

#### Data Gathering
The data source section of the UI allows you to define how the component will retrieve its input data. 
* **URI**: the URI represents the location of the resource where the JSON data is stored. It may be a text file on the filesystem or an URL to a webservice returning some JSON data. You can either input this value directly into the component (Direct Input) or you can specify a variable that is supposed to contain the URI at runtime. The UI allows you to control this parameter using the radio-buttons "Direct Input" and "Variable" and exposes a Browse button to choose a file or a variable. If you wish to retrieve the data from a URL, **you have to specify the whole URL with the http:// or https:// prefix**. Also for files on the file system, those should be prefixed with the **file:/// prefix**.
* **HttpMethod**: When an URL is specified, this settings controls what kind of HTTP request will be performed. HTTP parameters will be encoded accordingly.
* **HttpParameters**: If the data has to be retrieved via HTTP request, JSONSource allows you to specify one or more HTTP parameters. Like the URI mapping, HTTP parameters can be gathered via variable mappings, direct inputs and **from input columns**. This is very useful when data is provided using pagination.
* **COOKIE Variable**: if the webservice requires some state to be maintained among different requests, you may specify a variable where to store the HTTP sessions and cookies, so that future requests will use same session. That is not generally necessary with REST services, because they usually rely on access tokens.

#### Column mapping
As soon you as you start working with JSON, you'll need a way to extract only relevant data from it. The component itself helps in doing this providing an intuitive way of mapping expected input data into pre-defined output columns.
* **JSON Response Type**: defines what is the format of the JSON data recevied. Basically the webservice may return the data in form of an object (most of the case) or in form of a raw table (rarely). Please note that this does not refer to the type of data you wish to extract: even if you want to extract an inner array of objects but the response is an object, you should select "JSON Object." To make it clear, please consider the following.
Let's assume there is a webservice returning some personal information about a random guy and we want to extract the his interests.
{{
{
	"result": "OK",
	"data":[
		{
			"name": "Alberto",
			"surname": "Geniola",
			"age": 26,
			"preferred_color": "yellow",
			"interests": ["IT", "Music", "Soccer"](_IT_,-_Music_,-_Soccer_)
		}, {
			...
		}]
}
}}
The return type of the json is object (notice the initial bracket), so we need to specify "JsonObject" as response type. 
On the opposite, if the service returned the following data:
{{
[	
	{
		"name": "Alberto",
		"surname": "Geniola",
		"age": 26,
		"preferred_color": "yellow",
		"interests": ["IT", "Music", "Soccer"](_IT_,-_Music_,-_Soccer_)
	}, {
		...
	}
]
}}

we must specify "JsonArray" as response type (notice square the brackets at the beginning and at the end of the data).

* **Path to json array**: this settings allows you to limit the json parsing scope to a sub-branch of the whole response. This setting supports jsonpath syntax. For instance, let's consider the following json returned by the previous web-service.
{{
{
	"result": "OK",
	"data":[
		{
			"name": "Alberto",
			"surname": "Geniola",
			"age": 26,
			"preferred_color": "yellow",
			"interests": ["IT", "Music", "Soccer"](_IT_,-_Music_,-_Soccer_)
		}, {
			...
		}]
}
}}
If we want to parse all the users into the data array, we simply need to specify "**data**" as "path to json array/object". The rest of the column mapping will be limited to this scope.

* **Columns table**: this table is where you can define what json data should be outputted into SSIS rowset.
	* Json Field Name: is the name (or the jsonpath selector) of the json property to parse.
	* Max Length: represents the maximum length of the expected data. This will also be the maximum length of the associated output column.
	* OutputColumnName: represents the name that the output column will be given. 
	* ColumnType: specified the datatype of the outputcolumn. At the moment String/Number/Boolean/RawJson are supported. 
	
* **CopyColumns**: the listbox at the bottom represents all the available input columns detected. Each checked input column will be copied alongside the json data that was parsed. This is useful when performing queries to a webservice and it is important to remember the input parameters. Consider the case in which you have users' phone numbers and a service that returns the users' name-surname given their phone number. If you only configure the name and the surname values as output column, how will you match the users' phone numbers with their names-surnames again? By specifying the phone number as input and checking its checkbox, JSONSource component will output both the json parsed columns and the input selected columns in a unique row.

#### Advanced features
TBD

### Simple examples
TBD

### Complex Data structures and nested tables.
Please have a look at this discussion: [discussion:655407](discussion_655407)

### Power of **jsonpath**
TBD

### Real-World examples
TBD
