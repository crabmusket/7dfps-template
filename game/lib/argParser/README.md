# TorqueScript argument parser

## Example usage

    exec("lib/argParser/main.cs");

    $parser = ArgParser()
       .arg(q, "quiet")
       .arg(n, "no-warn")
       .arg(d, "difficulty");
    
    // Omit argument to parse $Game::argv
    $args = $parser.parse("-n --difficulty hard -q");

    echo($args.q);                        // echoes "___present___"
    echo($args.getFieldValue("no-warn")); // echoes "___present___"
    echo($args.difficulty);               // echoes "hard"
