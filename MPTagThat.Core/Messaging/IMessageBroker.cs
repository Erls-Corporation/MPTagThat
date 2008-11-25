﻿#region Copyright (C) 2007-2008 Team MediaPortal

/*
    Copyright (C) 2007-2008 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System.Collections.Generic;

namespace MPTagThat.Core
{
  /// <summary>
  /// Registration for all system message queues.
  /// </summary>
  public interface IMessageBroker
  {
    /// <summary>
    /// Get the message queue with the specified name.
    /// </summary>
    /// <param name="queueName">The name of the queue to return.</param>
    /// <returns>The queue with the specified name.</returns>
    IMessageQueue GetOrCreate(string queueName);

    /// <summary>
    /// Gets the names of all queues registered in this message broker.
    /// </summary>
    /// <returns>List of queue names.</returns>
    IList<string> Queues { get;}
  }
}