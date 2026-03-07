define GetNewTimer(dur)
   return [
	   _dur: dur
      _started: false
      _time: 0

      isComplete: (self) 
         return self._time >= self._dur

      update: (self deltaTime)
         if self.isComplete(self)
             self._time = self._time - deltaTime

      start: (self) {
         self._started = true
         self._time = 0
      }
    ]

# Returns the function itself and not its return value
return GetNewTimer
