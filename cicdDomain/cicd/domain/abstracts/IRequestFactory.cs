using cicdDomain.cicd.infrastructure;
using cicdDomain.cicd.infrastructure.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cicdDomain.cicd.domain.service
{
  public interface IRequestFactory
  {
    DomainRequest getRequestFrom(RequestPayload requestPayload);
  }
}
