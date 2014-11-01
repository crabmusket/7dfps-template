function ArgParser() {
   return new ScriptObject() {
      class = ArgParser;
      _argCount = 0;
   };
}

function ArgParser::arg(%this, %a, %b) {
   if (%a $= "" && %b $= "") {
      error("Cannot add argument with no name!");
   }

   %la = strlen(%a);
   %lb = strlen(%b);
   if (%la > 1 && %lb > 1) {
      error("Cannot determine which of" SPC %a SPC and SPC %b SPC "is the short argument");
   }

   %short = %la < %lb ? %a : %b;
   %long  = %la < %lb ? %b : %a;

   if(%long !$= "") {
      if(%this._getLongArg(%short) != -1) {
         error("Flag multiply defined: --" @ %long);
      }
   } else {
      error("Flag -" @ %short SPC "must have a long name!");
      return;
   }

   if(%short !$= "") {
      if(%this._getShortArg(%short) != -1) {
         error("Flag multiply defined" SPC "-" @ %short);
      }
   }

   %this._shorts[%this._argCount] = %short;
   %this._longs[%this._argCount] = %long;
   %this._argCount++;
   return %this;
}

function ArgParser::help(%this, %help) {
   %this._helps[%this.argCount] = %help;
   return %this;
}

function ArgParser::defaults(%this, %def) {
   %this._defaults[%this.argCount] = %def;
   return %this;
}

function ArgParser::parse(%this, %argString) {
   %this._reset();
   if (%argString $= "") {
      for (%i = 1; %i < $Game::argc-1; %i++) {
         %this._consumeArg($Game::argv[%i]);
      }
   } else {
      foreach$ (%arg in %argString) {
         %this._consumeArg(%arg);
      }
   }
   return %this._out;
}

// Runs the parser on $Game::argc and $Game::argv.
function ArgParser::parseDefaultArgs(%this) {
   %this._reset();
   return %this._out;
}

function ArgParser::_reset(%this) {
   // Object to hold all our parsed values.
   if (isObject(%this._out)) {
      %this._out.delete();
   }

   %this._out = new ScriptObject() {
      class = Args;
   };

   %this._currentArg = -1;
}

function ArgParser::_consumeArg(%this, %arg) {
   // Ignore things that are weird.
   if(%arg $= "--" || %arg $= "-") {
      return;
   }

   // Long format argument. Parse the body as a long name.
   if(strpos(%arg, "--") == 0) {
      %long = getSubStr(%arg, 2);
      %index = %this._getLongArg(%long);
      if (%index == -1) {
         error("Unknown command-line argument" SPC %arg);
      } else {
         %this._currentArg = %index;
         %this._setCurrentArgPresent(%index);
      }
   }

   // Short format argument. Check whether the next character is one of our
   // short names.
   else if(strpos(%arg, "-") == 0) {
      %short = getSubStr(%arg, 1, 1);
      %index = %this._getShortArg(%short);
      if (%index == -1) {
         error("Unknown command-line argument" SPC %arg);
      } else {
         %this._currentArg = %index;
         %this._setCurrentArgPresent(%index);
      }
   }

   // Things that aren't args become the 'contents' of the current arg.
   else {
      if (%this._currentArg != -1) {
         %this._appendCurrentArgValue(%this._currentArg, %arg);
      }
   }
}

function ArgParser::_setCurrentArgPresent(%this, %idx) {
   %sf = %this._shorts[%idx];
   %lf = %this._longs[%idx];
   foreach$ (%f in %sf SPC %lf) {
      if (%this._out.getFieldValue(%f) $= "") {
         %this._out.setFieldValue(%f, "___present___");
      }
   }
}

function ArgParser::_appendCurrentArgValue(%this, %idx, %val) {
   %sf = %this._shorts[%idx];
   %lf = %this._longs[%idx];
   foreach$ (%f in %sf SPC %lf) {
      %current = %this._out.getFieldValue(%f);
      if (%current $= "" || %current $= "___present___") {
         %this._out.setFieldValue(%f, %val);
      } else {
         %this._out.setFieldValue(%f, %current SPC %val);
      }
   }
}

function ArgParser::_getShortArg(%this, %short) {
   for(%i = 0; %i < %this._argCount; %i++) {
      if(%this._shorts[%i] $= %short) {
         return %i;
      }
   }
   return -1;
}

function ArgParser::_getLongArg(%this, %long) {
   for(%i = 0; %i < %this._argCount; %i++) {
      if(%this._longs[%i] $= %long) {
         return %i;
      }
   }
   return -1;
}
