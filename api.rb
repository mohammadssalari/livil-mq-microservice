# frozen_string_literal: true

require 'json'
require 'sinatra'

# Pseudo-summarizes a given text
#
# @param input [String] The input text
# @return [String] A single sentence taken from the input
def generate_synopsis(input)
  input
    .split(/[\.,?]/)
    .reject(&:empty?)
    .sample
    .strip
end

# Set the default content type for responses
before do
  content_type :json
end

# Extract the input from the JSON in the request body
#
# @return [String] The response containing the summarized text
post '/analyze' do
  request.body.rewind
  parsed = JSON.parse(request.body.read)

  synopsis = generate_synopsis(parsed['content'])

  { synopsis: synopsis }.to_json
end
