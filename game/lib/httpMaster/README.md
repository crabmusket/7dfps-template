# TorqueScript HTTP master server interface

This script provides a simple API for interfacing with an HTTP master server.
Requires Torque 3D 3.7 or greater, because `HTTPObject::post` is [broken][] in earlier versions.

[broken]: https://github.com/GarageGames/Torque3D/issues/918

## Usage - clients

To query a list of servers, set the master server location and URI you expect:

    HTTPMaster.masterLocation = "www.myserver.com:80";
    HTTPMaster.gameListURI = "/gameList";

To get the results, you have three options.
You can call `query` with a single parameter, which will cause the function with that name to be called once the results are available.

    function getResults(%query) {
       echo(%query.contents);
    }
    HTTPMaster.query(getResults);

Second, you can call `query` with an object and a method name:

    new ScriptObject(CallbackReceiver);
    function CallbackReceiver::getResults(%query) {
       echo(%query.contents);
    }
    HTTPMaster.query(CallbackReceiver, getResults);

Finally, you can call `query` with no parameters, in which case, upon query completion, an event will be posted to the `HTTPMasterEvents` queue:

    new ScriptObject(EventListener);
    HTTPMasterEvents.subscribe(EventListener, EvtQueryDone);
    function EventListener::onEvtQueryDone(%this, %query) {
       echo(%query.contents);
    }
    HTTPMaster.query();

## Usage - servers

