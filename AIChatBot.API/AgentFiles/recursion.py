def factorial(n):
  if n == 0:
    return 1
  else:
    return n * factorial(n-1)

# Example usage
num = 5
print("The factorial of", num, "is", factorial(num))

# Explanation:
# Recursion is a programming technique where a function calls itself within its own definition.
# In this example, the factorial function calculates the factorial of a number by calling itself with a smaller input (n-1).
# The base case (n == 0) stops the recursion and returns a value (1).
# Each recursive call breaks down the problem into smaller subproblems until the base case is reached.
# The results of the subproblems are then combined to solve the original problem.