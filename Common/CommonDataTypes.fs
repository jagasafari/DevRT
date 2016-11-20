namespace Common

type Result<'a, 'b> = | Success of 'a | Failure of 'b
