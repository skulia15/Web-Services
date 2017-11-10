# Web-Services
Projects for the Web Services course at Reykjavik University

Includes projects implemented in .NET and Node.js

## Getting Started
.Net Core
npm or yarn package managers and a recent version of Node.js

### Installing
For each project in node you will need to run 'npm install' or 'yarn install'

## Authors

* **Skúli Arnarsson** 
* **Andri Karel Júlíusson** 
* **Smári Björn Gunnarsson** 

# Course Notes
## Client and Server
### Server side rendering
Pros
* The initial page of a website loads faster
* It’s great for static sites
* It’s rendering is great for SEO (search engine optimization)
* The client can be very light weight

Cons
* Frequent server requests
* Slow rendering
* Full page reloads
* Non-rich (non-dynamic) site interactions

### Client side rendering
Pros
* The server can just send DATA (e.g on JSON format)!
* Each client can render the data differently
* The programming language of the clients doesn´t matter
* Rich site interactions (dynamic interactions)
* Faster rendering
* Web apps!

Cons
* websites won’t be able to load until ALL the JavaScript is
downloaded to the browser
* Bad for SEO
* Initial load slow

## Web services
A web service is:
* Software that is available over the Internet or a private (intranet)
network
* uses a *standardized* messaging system
*  not tied to any one operating system or programming language

## API
Application Programming Interface

A web API is an API that is accessible over the internet

concept, not a technology+

## Different types of web services
SOAP
* xml

WCF (Windows communication foundation)

REST - Representational state transfer
* Usage of standard HTTP
* Different data formats
* performance and scalability
* Main Characteristics:
    * Client and Server are separated
    * Stateless -  State is sent with each request
    * Cacheable
    * Hypertext Driven

## Status Codes
Most common response status codes:

200: OK, request was sucessful

201: Created, Post request created an entity successfully

204: No content, When request doesn't return anything

301: Redirect / Moved permanently, The resource was moved

400: Bad request, Error on clients behalf. Request was incorrect.

401: Unauthorized, Client must specify authentication http header

402: Payment Required

403: Forbidden, The server knows clients idenity but the client isn't allowed to perform this action

404: Not found

412: Precondition failed, Can be used when a particular property is required, but wasn't provided by the client

500: Server error. Nothing a client can do

503: Service unavailable
