import json
from json import JSONEncoder
import datetime
from datetime import date, time, datetime, timedelta

class Hardware():

    def __init__(self, item):
        self.name = item[0]
        self.ip = item[1]
        self.platform = item[2]
        self.lease_duration = item[3]
        self.lease_date = item[4]
        self.is_leased = self._calculate_leased()
    
    def _calculate_leased(self):
        if self.lease_date is None:
            return False
        return self.lease_date + timedelta(minutes=self.lease_duration) > datetime.now()
    
    def to_json(self):
        d = self.__dict__
        if self.lease_date is not None: d["lease_date"] = self.lease_date.strftime("%Y-%m-%dT%H:%M:%S")
        if not self.is_leased:
            d["lease_date"] = None
            d["lease_duration"] = 0 
        return d
    
def _default(self, obj):
    return getattr(obj.__class__, "to_json", _default.default)(obj)

_default.default = JSONEncoder().default
JSONEncoder.default = _default

