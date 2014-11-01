# TorqueScript argument parser

## Example usage

    exec("lib/argParser/main.cs");

    $parser = ArgParser()
       .arg(q, "quiet")
       .arg(n, "no-warn")
       .arg(d, "difficulty");
    
    // Omit argument to parse $Game::argv
    $args = $parser.parse("-n -q --difficulty hard");

    echo($args.difficulty);
