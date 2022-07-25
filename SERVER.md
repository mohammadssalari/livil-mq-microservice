# Mock API server

## Requirements

- Ruby 3.x
- Rubygems & Bundler


## Setup

- Install Ruby
- Install Bundler: `gem install bundler`
- Install project dependencies: `bundle install`

## Running & Testing 

#### Start the server: 
```bash
bundle exec ruby api.rb
```

#### Test the server: 
```bash
curl -X POST --data '{"content": "a long, long piece of text. maybe more sentences"}' https://localhost:4567/analyze
```


